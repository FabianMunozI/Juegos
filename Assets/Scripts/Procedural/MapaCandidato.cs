using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using SVS.AI;
using System.Linq;

namespace SVS.ChessMaze
{
    public class MapaCandidato 
    {
        private GrillaMapa grilla;
        private bool[] obstaclesArray = null;
        private Vector3 startPoint, exitPoint;
        private List<Vector3> path = new List<Vector3>();

        // variable RIO
        private List<Vector3> rio = new List<Vector3>(); //! rio es el homologo a path
        private Vector3 inicioRio, finalRio;

        // algoritmo genetico
        private List<Vector3> cornerList;
        private int cornerNearEachOtherCount;

        // algoritmo genetico RIO
        private List<Vector3> listaCurbasRio;
        private int cornerNearEachOtherCountRio;

    

        public GrillaMapa Grilla { get => grilla;}
        public global::System.Boolean[] ObstaclesArray { get => obstaclesArray;}

        public MapaCandidato(GrillaMapa grilla)
        {
            this.grilla = grilla;
        }

        public void CrearMapa(Vector3 startPosition, Vector3 exitPosition, Vector3 inicioRIO, Vector3 finalRIO, bool autoRepair = false)
        {
            this.startPoint = startPosition;
            this.exitPoint = exitPosition;
            this.inicioRio = inicioRIO;
            this.finalRio = finalRIO;
            obstaclesArray = new bool[grilla.Ancho*grilla.Largo];

            //PlaceObstaculos();

            FindPath();

            // RIO
            GenerarRio();

            Debug.Log("After Rio");
            //foreach( Vector3 bloque in this.rio)
            //{
            //    Debug.Log("{ " + bloque.x + " , " + bloque.y + " , " + bloque.z + " }");
            //}

            if (autoRepair)
            {
                Reparar();
            }
        }

        public void FindPath()
        {
            this.path = Astar.GetPath(startPoint,exitPoint,obstaclesArray,grilla);
            this.cornerList = GetListCorners(this.path, this.startPoint);
            this.cornerNearEachOtherCount = CarlcularCorners(this.cornerList);
        }

        //RIO
        public void GenerarRio() //! homologo a  FindPath
        {
            this.rio = AstarRio.GetPath(inicioRio, finalRio, obstaclesArray, grilla, this.path, cornerList); //! 
            //this.rio = CreacionRio.GetRio(inicioRio, obstaclesArray, grilla, path, cornerList); // gritito
            this.listaCurbasRio = GetListCorners(this.rio, this.inicioRio);
            this.cornerNearEachOtherCountRio = CarlcularCorners(this.listaCurbasRio);
        }

        private List<Vector3> GetListCorners(List<Vector3> path, Vector3 posicionInicio) //! path puede ser path o rio porque uno se lo pasa
        {
            List<Vector3> pathStart = new List<Vector3>(path);
            pathStart.Insert(0,posicionInicio);
            List<Vector3> cornerPositions = new List<Vector3>();
            if (pathStart.Count <= 0 )
            {
                return cornerPositions;
            }
            for (int i = 0 ; i < pathStart.Count - 2 ; i++)
            {
                //buscando corners
                if (pathStart[i+1].x > pathStart[i].x || pathStart[i+1].x < pathStart[i].x)
                {
                    if (pathStart[i + 2].z != pathStart[i + 1].z || pathStart[i + 2].z < pathStart[i + 1].z)
                    {
                        cornerPositions.Add(pathStart[i+1]);
                    }
                }
                else if (pathStart[i+1].z > pathStart[i].z || pathStart[i+1].z < pathStart[i].z)
                {
                    if (pathStart[i + 2].x != pathStart[i + 1].x || pathStart[i + 2].x < pathStart[i + 1].x)
                    {
                        cornerPositions.Add(pathStart[i+1]);
                    }
                }
            }
            return cornerPositions;
        }


        private int CarlcularCorners(List<Vector3> cornerList)
        {
            int cornersNear = 0;
            for (int i = 0 ; i < cornerList.Count - 1 ; i++)
            {
                if (Vector3.Distance(cornerList[i], cornerList[i + 1]) <= 1)
                {
                    cornersNear++;
                }
            }
            return cornersNear;
        }

        private bool CheckIfPosotionCanBeObstacle(Vector3 position)
        {
            if (position == startPoint || position == exitPoint)
            {
                return false;
            }
            if (position == inicioRio ||  position == finalRio)
            {
                return false;
            }
            int index = grilla.CalcularIndxCoordns(position.x,position.z);
            return obstaclesArray[index] == false;
        }   

        public MapData ReturnMapData()
        {
            return new MapData
            {
                obstaclesArray = this.obstaclesArray,
                startPosition = startPoint,
                exitPosition = exitPoint,
                path = this.path,
                cornerList = this.cornerList,
                cornerNearEachOther = this.cornerNearEachOtherCount,
                //RIO
                inicioRio = this.inicioRio,
                finalRio = this.finalRio,
                rio = this.rio,
                listaCurbasRio = this.listaCurbasRio,
                cornerNearEachOtherRio = this.cornerNearEachOtherCountRio
            };
        }

        public List<Vector3> Reparar()
        {
            int numeroObstaculos = obstaclesArray.Where(obstacle => obstacle).Count();
            List<Vector3> listaObstaculosRemover = new List<Vector3>();
            if (path.Count <= 0)
            {
                do
                {
                    int obstaculoIndexRemuv = Random.Range(0,numeroObstaculos);
                    for (int i = 0 ; i < obstaclesArray.Length ;  i++)
                    {
                        if (obstaclesArray[i])
                        {
                            if(obstaculoIndexRemuv == 0)
                            {
                                obstaclesArray[i] = false;
                                listaObstaculosRemover.Add(grilla.CalcularIndxCoordns(i));
                                break;
                            }
                            obstaculoIndexRemuv--;
                        }
                    }
                    FindPath();
                }while(this.path.Count <= 0);
            }

            //RIO
            if (rio.Count <= 0)
            {
                do
                {
                    int obstaculoIndexRemuv = Random.Range(0,numeroObstaculos);
                    for (int i = 0 ; i < obstaclesArray.Length ;  i++)
                    {
                        if (obstaclesArray[i])
                        {
                            if(obstaculoIndexRemuv == 0)
                            {
                                obstaclesArray[i] = false;
                                listaObstaculosRemover.Add(grilla.CalcularIndxCoordns(i));
                                break;
                            }
                            obstaculoIndexRemuv--;
                        }
                    }
                    GenerarRio();
                }while(this.rio.Count <= 0);
            }

            foreach (var obstaclePosition in listaObstaculosRemover)
            {
                if (path.Contains(obstaclePosition) == false && rio.Contains(obstaclePosition) == false) //! OJITO!! REVISAR SI ERA && O ||
                {
                    int index = grilla.CalcularIndxCoordns(obstaclePosition.x,obstaclePosition.z);
                    obstaclesArray[index] = true;
                }
            }
            return listaObstaculosRemover;
        }

        public void AddMutacion(double mutacionRate)
        {
            int numeroItems = (int)(obstaclesArray.Length*mutacionRate);
            while (numeroItems > 0)
            {
                int randomIndex = Random.Range(0, obstaclesArray.Length);
                obstaclesArray[randomIndex] = !obstaclesArray[randomIndex];
                numeroItems--;
            }
        }

        public MapaCandidato DeepClone()
        {
            return new MapaCandidato(this);
        }

        public MapaCandidato(MapaCandidato mapaCandidato)
        {
            this.grilla = mapaCandidato.grilla;
            this.startPoint = mapaCandidato.startPoint;
            this.exitPoint = mapaCandidato.exitPoint;
            this.obstaclesArray = (bool[])mapaCandidato.obstaclesArray.Clone();
            this.cornerList = new List<Vector3>(mapaCandidato.cornerList);
            this.cornerNearEachOtherCount = mapaCandidato.cornerNearEachOtherCount;
            this.path = new List<Vector3>(mapaCandidato.path);

            //RIO

            this.rio = new List<Vector3>(mapaCandidato.rio);
            this.inicioRio = mapaCandidato.inicioRio;
            this.finalRio = mapaCandidato.finalRio;
            this.listaCurbasRio = new List<Vector3>(mapaCandidato.listaCurbasRio);
            this.cornerNearEachOtherCountRio = mapaCandidato.cornerNearEachOtherCountRio;
        }

        public void PlaceObstaculos(int i, bool isObst)
        {
            obstaclesArray[i] = isObst;
        }

        public bool isObstacleAt(int i)
        {
            return obstaclesArray[i];
        }

    }

}



