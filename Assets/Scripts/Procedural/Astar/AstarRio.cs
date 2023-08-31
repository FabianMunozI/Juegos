using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVS.ChessMaze;
using System.Linq;

namespace SVS.AI
{
    public static class AstarRio
    {
        public static List<Vector3> GetPath(Vector3 start, Vector3 exit, bool[] obstArray, GrillaMapa grilla, List<Vector3> path, List<Vector3> curbasCamino)
        {
            Debug.Log("In GetPath");
            //foreach (Vector3 bloque in path)
            //{
            //    Debug.Log("{ " + bloque.x + " , " + bloque.y + " , " + bloque.z + " }");
            //}

            //start = MapHelper.RandomBorde(grilla, start);
            //exit = MapHelper.RandomBorde(grilla, start);

            //while (path.Contains(start))
            //{
            //    Debug.Log("Re-assigning Rio Start");
            //    Debug.Log("start: { " + start.x + " , " + start.y + " , " + start.z + " }");
            //    Debug.Log("end  : { " + exit.x + " , " + exit.y + " , " + exit.z + " }");
            //    start = MapHelper.RandomBorde(grilla, start);
            //     TODO: check this code if grid is not symmetrical
            //    for (int i = 0; i < grilla.Ancho; i++)
            //    {
            //        for (int j = 0; j < grilla.Largo; j++)
            //        {
            //            if (grilla.IsCeldaTaken(i, j) && (grilla.GetCelda(i, j).TipoObjeto == TipoObjCelda.InicioRio))
            //            {
            //                Debug.Log("unsetting start of rio");
            //                grilla.UnsetCelda(i, j);
            //                break;
            //            }
            //        }
            //    }
            //}

            //while (path.Contains(exit))
            //{
            //    Debug.Log("Re-assigning Rio Exit");
            //    Debug.Log("start: { " + start.x + " , " + start.y + " , " + start.z + " }");
            //    Debug.Log("end  : { " + exit.x + " , " + exit.y + " , " + exit.z + " }");
            //    exit = MapHelper.RandomBorde(grilla, start);

            //     TODO: Check this code if grid is not symmetrical
            //    for (int i = 0; i < grilla.Ancho; i++)
            //    {
            //        for (int j = 0; j < grilla.Largo; j++)
            //        {
            //            if (grilla.IsCeldaTaken(i, j) && (grilla.GetCelda(i, j).TipoObjeto == TipoObjCelda.FinalRio))
            //            {
            //                Debug.Log("Unsetting end of rio");
            //                grilla.UnsetCelda(i, j);
            //                break;
            //            }
            //        }
            //    }
            //}

            //grilla.SetCelda(start.x, start.z, TipoObjCelda.InicioRio, true);
            //grilla.SetCelda(exit.x, exit.z, TipoObjCelda.FinalRio, true);

            VertexPositionRio startVertex = new VertexPositionRio(start);
            VertexPositionRio exitVertex = new VertexPositionRio(exit);

            List<Vector3> rio = new List<Vector3>();
   

            List<VertexPositionRio> openedList = new List<VertexPositionRio>();
            HashSet<VertexPositionRio> closedList = new HashSet<VertexPositionRio>();

            startVertex.costoEstimado = ManhatanDistance(startVertex,exitVertex);

            openedList.Add(startVertex);

            VertexPositionRio currentVertex = null;

            //evita que el rio se junte con el camino nuevamente antes de 3 bloques
            int alejateRio = 0;


            while ( openedList.Count > 0)
            {
                openedList.Sort();
                currentVertex = openedList[0];

                if (currentVertex.Equals(exitVertex))
                {
                    while ( currentVertex != startVertex)
                    {
                        if(path.Contains(currentVertex.Position) && alejateRio < 1) // añadir Puente
                        {
                            if(!curbasCamino.Contains(currentVertex.Position)) // hay que reisar si es una curva, porque el rio no puede chocar con las curbas COMO SABER SI ES UNA CURBA??
                            {
                                rio.Add(currentVertex.Position);
                                alejateRio = 3;
                                if (currentVertex.exVertex != null)
                                {
                                    Vector3 dif = currentVertex.Position - currentVertex.exVertex.Position;
                                    if (dif.x != 0)
                                    {
                                        if (dif.x > 0)
                                        {
                                            rio.Add(currentVertex.Position + new Vector3(1, 0, 0));
                                        }
                                        else
                                        {
                                            rio.Add(currentVertex.Position + new Vector3(-1, 0, 0));
                                        }
                                    }
                                    else if (dif.z != 0)
                                    {
                                        if (dif.z > 0)
                                        {
                                            rio.Add(currentVertex.Position + new Vector3(0, 0, 1));
                                        }
                                        else
                                        {
                                            rio.Add(currentVertex.Position + new Vector3(0, 0, -1));
                                        }
                                    }
                                    break;
                                }
                            }
                            // hay que hacer que entonces el rio siga recto / (hacia donde no hay camino) al menos un bloque 
                        }else if(!path.Contains(currentVertex.Position))
                        {
                            rio.Add(currentVertex.Position);
                            alejateRio = 3;
                            currentVertex.exVertex = currentVertex;
                        }
                        alejateRio--;
                        currentVertex = currentVertex.previousVertex;
                    }
                    rio.Reverse();
                    break;
                }
                var arrayVecinos = FindVecinos(currentVertex,grilla,obstArray);
                foreach (var vecino in arrayVecinos)
                {
                    if ( vecino == null || closedList.Contains(vecino))
                    {
                        continue;
                    }
                    if (vecino.IsTaken == false)
                    {
                        var costoTotal = currentVertex.costoTotal + 1;
                        var vecinoCostoEstimado = ManhatanDistance(vecino,exitVertex);
                        vecino.costoTotal = costoTotal;
                        vecino.previousVertex = currentVertex;
                        vecino.costoEstimado = costoTotal + vecinoCostoEstimado;
                        if (openedList.Contains(vecino) == false)
                        {
                            openedList.Add(vecino);
                        }
                    }
                }
                closedList.Add(currentVertex);
                openedList.Remove(currentVertex);
            }

            return rio;
        }

        private static VertexPositionRio[] FindVecinos(VertexPositionRio currentVertex, GrillaMapa grilla, bool[] obstArray)
        {
            VertexPositionRio[] arrayVecinos = new VertexPositionRio[4]; //max 4 vecinos

            int arrayIndex = 0;
            foreach ( var posiblesVecinos in VertexPositionRio.posiblesVecinos)
            {
                Vector3 position = new Vector3(currentVertex.X + posiblesVecinos.x, 0, currentVertex.Z + posiblesVecinos.y);
                if (grilla.IsCeldaValid(position.x, position.z))
                {
                    int index = grilla.CalcularIndxCoordns(position.x, position.z);
                    arrayVecinos[arrayIndex] = new VertexPositionRio(position,obstArray[index]);
                    arrayIndex++;
                }
            }
            return arrayVecinos;
        }

        private static float ManhatanDistance(VertexPositionRio startVertex, VertexPositionRio exitVertex)
        {
            return Mathf.Abs(startVertex.X - exitVertex.X) + Mathf.Abs(startVertex.Z - exitVertex.Z);
        }
    }
}