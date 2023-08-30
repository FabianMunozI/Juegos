using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SVS.ChessMaze
{
    public class VizualisadorMapa : MonoBehaviour
    {
        private Transform parent;

        public GameObject roadStraight, roadTileCorner, tileEmpty, startTile,exitTile;
        public GameObject[] enviromentTiles; // obstaculos 

        Dictionary<Vector3,GameObject> diccionarioObstaculos = new Dictionary<Vector3,GameObject>();
        List<GameObject> pisoExtra = new List<GameObject>();

        //RIO
        public GameObject rioRecto, curbaRio, puente, inRio, finRio;

        private void Awake()
        {
            parent = this.transform;
        }

        public void VisualizarMapa(GrillaMapa grilla, MapData data)
        {
            for ( int i = 0 ; i < data.path.Count ; i++)
            {
                var position = data.path[i];
                if (position != data.exitPosition)
                {
                    grilla.SetCelda(position.x,position.z,TipoObjCelda.Road);
                }
            }

            
            //RIO
            for ( int j = 0 ; j < data.rio.Count ; j++)
            {
                var position2 = data.rio[j];
                if (position2 != data.finalRio)
                {
                    if (grilla.GetCelda(position2.x, position2.z).TipoObjeto == TipoObjCelda.Road) //! REVISAR SI HAY CAMINO EN ESA CELDA
                    {
                        grilla.SetCelda(position2.x,position2.z,TipoObjCelda.RoadPuente);
                    }
                    else
                    {
                        grilla.SetCelda(position2.x,position2.z,TipoObjCelda.Rio);
                    }
                }
            }

            for (int col = 0 ; col < grilla.Ancho ; col++)
            {
                for (int row = 0 ; row < grilla.Largo ; row++)
                {
                    var celda = grilla.GetCelda(col,row);
                    var position = new Vector3(celda.X, 0, celda.Z);

                    var index = grilla.CalcularIndxCoordns(position.x,position.z);
                    if (data.obstaclesArray[index] == true && celda.IsTaken == false)
                    {
                        celda.TipoObjeto = TipoObjCelda.Obstacle;
                    }
                    Direccion direccionPrevia = Direccion.None;
                    Direccion direccionNext = Direccion.None;

                    //RIO
                    Direccion direccionPreviaRio = Direccion.None;
                    Direccion direccionNextRio = Direccion.None;

                    switch (celda.TipoObjeto)
                    {
                        case TipoObjCelda.Empty:
                            CrearIndicador(position,tileEmpty);
                            break;
                        case TipoObjCelda.Road:
                            //CrearIndicador(position,roadStraight);

                            if (data.path.Count > 0)
                            {
                                direccionPrevia = GetDireccionCeldaPrevia(position,data);
                                direccionNext = GetDireccionCeldaNext(position, data);
                            }
                            if (direccionPrevia == Direccion.Arriba && direccionNext == Direccion.Derecha || direccionPrevia == Direccion.Derecha && direccionNext == Direccion.Arriba)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position,roadTileCorner,Quaternion.Euler(0,180,0));
                            } else if (direccionPrevia == Direccion.Derecha && direccionNext == Direccion.Abajo || direccionPrevia == Direccion.Abajo && direccionNext == Direccion.Derecha)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position,roadTileCorner,Quaternion.Euler(0,270,0));
                            }else if (direccionPrevia == Direccion.Abajo && direccionNext == Direccion.Izquierda || direccionPrevia == Direccion.Izquierda && direccionNext == Direccion.Abajo)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position,roadTileCorner);
                            }else if (direccionPrevia == Direccion.Izquierda && direccionNext == Direccion.Arriba || direccionPrevia == Direccion.Arriba && direccionNext == Direccion.Izquierda)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position,roadTileCorner,Quaternion.Euler(0,90,0));
                            }else if (direccionPrevia == Direccion.Derecha && direccionNext == Direccion.Izquierda || direccionPrevia == Direccion.Izquierda && direccionNext == Direccion.Derecha)
                            {
                                var poner2 = position + new Vector3(.4801336f, .56981165f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                rotacion2 = Quaternion.Euler(0,90,0);
                                var elemento2 = Instantiate(roadStraight, poner2, rotacion2);
                                elemento2.transform.parent = parent;
                                //pisoExtra.Add(elemento2); //! qué pasa si la borro ?
                                diccionarioObstaculos.Add(position,elemento2);
                            }else
                            {
                                var poner2 = position + new Vector3(.4801336f, .5446233f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                var elemento2 = Instantiate(roadStraight, poner2, rotacion2);
                                elemento2.transform.parent = parent;
                                //pisoExtra.Add(elemento2);
                                diccionarioObstaculos.Add(position,elemento2);
                            }


                            break;
                        case TipoObjCelda.Obstacle:
                            int randomIndex = Random.Range(0, enviromentTiles.Length);

                            var poner = position + new Vector3(.5f, .5f, .5f);
                            Quaternion rotacion = new Quaternion();
                            var elemento = Instantiate(tileEmpty, poner, rotacion);
                            elemento.transform.parent = parent;
                            pisoExtra.Add(elemento);

                            CrearIndicador(position, enviromentTiles[randomIndex]);
                            break;
                        case TipoObjCelda.Start:
                            if(data.path.Count > 0)
                            {
                                direccionNext = GetDireccionVector(data.path[0],position);
                            }
                            if (direccionNext == Direccion.Derecha || direccionNext == Direccion.Izquierda)
                            {
                                CrearIndicador(position, startTile,Quaternion.Euler(0,90,0));
                            }
                            else
                            {
                                CrearIndicador(position,startTile);
                            }
                            break;
                        case TipoObjCelda.Exit:
                            position = position + new Vector3(0, .01f, 0);
                            if (data.path.Count > 0)
                            {
                                direccionPrevia = GetDireccionCeldaPrevia(position,data);
                            }
                            switch (direccionPrevia)
                            {
                                case Direccion.Derecha:
                                    CrearIndicador(position,exitTile,Quaternion.Euler(0,90,0));
                                    break;
                                case Direccion.Izquierda:
                                    CrearIndicador(position,exitTile,Quaternion.Euler(0,-90,0));
                                    break;
                                case Direccion.Abajo:
                                    CrearIndicador(position,exitTile,Quaternion.Euler(0,180,0));
                                    break;
                                default:
                                    CrearIndicador(position,exitTile);
                                    break;
                            }
                            break;

                        //RIO    
                        case TipoObjCelda.RoadPuente:

                            if (data.path.Count > 0)
                            {
                                direccionPreviaRio = GetDireccionCeldaPreviaRio(position,data);
                                direccionNextRio = GetDireccionCeldaNextRio(position, data);
                            }

                            if (direccionPreviaRio == Direccion.Derecha || direccionNextRio == Direccion.Izquierda || direccionPreviaRio == Direccion.Izquierda || direccionNextRio == Direccion.Derecha)
                            {
                                var posPuente = position + new Vector3(.5f, .5f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                var elemento2 = Instantiate(puente, posPuente, rotacion2);
                                elemento2.transform.parent = parent;

                                //Rio abajo de puente
                                Quaternion rotacionRio = Quaternion.Euler(0,90,0);
                                var rioPuente = Instantiate(rioRecto, posPuente, rotacionRio);
                                rioPuente.transform.parent = parent;
                                pisoExtra.Add(rioPuente);

                                diccionarioObstaculos.Add(position,elemento2);
                            }else
                            {
                                var posPuente = position + new Vector3(.5f, .5f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                rotacion2 = Quaternion.Euler(0,90,0);
                                var elemento2 = Instantiate(puente, posPuente, rotacion2);
                                elemento2.transform.parent = parent;

                                //Rio abajo de puente
                                Quaternion rotacionRio = new Quaternion() ;
                                var rioPuente = Instantiate(rioRecto, posPuente, rotacionRio);
                                rioPuente.transform.parent = parent;
                                pisoExtra.Add(rioPuente);

                                diccionarioObstaculos.Add(position,elemento2);
                            }

                            break;
                        case TipoObjCelda.Rio:

                            if (data.rio.Count > 0)
                            {
                                direccionPreviaRio = GetDireccionCeldaPreviaRio(position,data);
                                direccionNextRio = GetDireccionCeldaNextRio(position, data);
                            }
                            if (direccionPreviaRio == Direccion.Arriba && direccionNextRio == Direccion.Derecha || direccionPreviaRio == Direccion.Derecha && direccionNextRio == Direccion.Arriba)
                            {
                                //position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position,curbaRio,Quaternion.Euler(0,180,0));
                            } else if (direccionPreviaRio == Direccion.Derecha && direccionNextRio == Direccion.Abajo || direccionPreviaRio == Direccion.Abajo && direccionNextRio == Direccion.Derecha)
                            {
                                CrearIndicador(position,curbaRio,Quaternion.Euler(0,270,0));
                            }else if (direccionPreviaRio == Direccion.Abajo && direccionNextRio == Direccion.Izquierda || direccionPreviaRio == Direccion.Izquierda && direccionNextRio == Direccion.Abajo)
                            {
                                CrearIndicador(position,curbaRio);
                            }else if (direccionPreviaRio == Direccion.Izquierda && direccionNextRio == Direccion.Arriba || direccionPreviaRio == Direccion.Arriba && direccionNextRio == Direccion.Izquierda)
                            {
                                CrearIndicador(position,curbaRio,Quaternion.Euler(0,90,0));
                            }else if (direccionPreviaRio == Direccion.Derecha && direccionNextRio == Direccion.Izquierda || direccionPreviaRio == Direccion.Izquierda && direccionNextRio == Direccion.Derecha)
                            {
                                position = position + new Vector3(.5f, 0.5f, .5f); //! comentar si no sale
                                Quaternion rotacion2 = new Quaternion();
                                rotacion2 = Quaternion.Euler(0,90,0);
                                //var elemento2 = Instantiate(rioRecto, poner2, rotacion2);
                                var elemento2 = Instantiate(rioRecto, position, rotacion2, parent);
                                //elemento2.transform.parent = parent;
                                diccionarioObstaculos.Add(position,elemento2);
                            }else
                            {
                                position = position + new Vector3(.5f, 0.5f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                //var elemento2 = Instantiate(rioRecto, poner2, rotacion2);
                                var elemento2 = Instantiate(rioRecto, position, rotacion2, parent);
                                //elemento2.transform.parent = parent;
                                diccionarioObstaculos.Add(position,elemento2);
                            }

                            break;
                        case TipoObjCelda.InicioRio:

                            if(data.rio.Count > 0)
                            {
                                direccionNextRio = GetDireccionVector(data.rio[0], position);
                            }

                            switch(direccionNextRio)
                            {
                                case (Direccion.Derecha):
                                    CrearIndicador(position, inRio, Quaternion.Euler(0, 270, 0));
                                    break;
                                case (Direccion.Izquierda):
                                    CrearIndicador(position, inRio, Quaternion.Euler(0, 90, 0));
                                    break;
                                case (Direccion.Abajo):
                                    CrearIndicador(position, inRio);
                                    break;
                                default:
                                    CrearIndicador(position, inRio, Quaternion.Euler(0, 180, 0));
                                    break;
                            }
                          
                            break;
                        case TipoObjCelda.FinalRio:

                            if (data.rio.Count > 0)
                            {
                                direccionPreviaRio = GetDireccionCeldaPreviaRio(position,data);
                            }
                            switch (direccionPreviaRio)
                            {
                                case (Direccion.Derecha):
                                    CrearIndicador(position,finRio, Quaternion.Euler(0, 270, 0));
                                    break;
                                case (Direccion.Izquierda):
                                    CrearIndicador(position,finRio, Quaternion.Euler(0, 90, 0));
                                    break;
                                case (Direccion.Abajo):
                                    CrearIndicador(position, finRio);
                                    break;
                                default:
                                    CrearIndicador(position,finRio, Quaternion.Euler(0, 180, 0));
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private Direccion GetDireccionCeldaNext(Vector3 position, MapData data)
        {
            int index = data.path.FindIndex(async => async == position);
            var nextCellPosition = data.path[index + 1]; //sabemos que existe index + 1 
            return GetDireccionVector(nextCellPosition,position);
        }

        private Direccion GetDireccionCeldaNextRio(Vector3 position, MapData data)
        {
            int index = data.rio.FindIndex(async => async == position);
            var nextCellPosition = data.rio[index + 1]; //sabemos que existe index + 1 
            return GetDireccionVector(nextCellPosition,position);
        }

        private Direccion GetDireccionCeldaPrevia(Vector3 position, MapData data)
        {
            var index = data.path.FindIndex(async => async == position);
            var previaCeldaPos = Vector3.zero;
            if (index > 0) // no es la primera posicion en el path
            {
                previaCeldaPos = data.path[index-1];
            }
            else
            {
                previaCeldaPos = data.startPosition;
            }
            return GetDireccionVector(previaCeldaPos,position);
        }

        private Direccion GetDireccionCeldaPreviaRio(Vector3 position, MapData data)
        {
            var index = data.rio.FindIndex(async => async == position);
            var previaCeldaPos = Vector3.zero;
            if (index > 0) // no es la primera posicion en el path
            {
                previaCeldaPos = data.rio[index-1];
            }
            else
            {
                previaCeldaPos = data.inicioRio;
            }
            return GetDireccionVector(previaCeldaPos,position);
        }

        private Direccion GetDireccionVector(Vector3 positionGo, Vector3 position)
        {
            if (positionGo.x > position.x)
            {
                return Direccion.Derecha;
            }else if ( positionGo.x < position.x)
            {
                return Direccion.Izquierda;
            }else if (positionGo.z < position.z)
            {
                return Direccion.Abajo;
            }
            return Direccion.Arriba;
        }

        private void CrearIndicador(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion())
        {
            var placementPosition = position + new Vector3(.5f, .5f, .5f);
            var element = Instantiate(prefab, placementPosition, rotation);
            element.transform.parent = parent;
            diccionarioObstaculos.Add(position,element);
        }


        public void LimpiarMapa()
        {

            Debug.Log( "diccionario " + diccionarioObstaculos);
            foreach (var obstacle in diccionarioObstaculos.Values)
            {
                Destroy(obstacle);
            }
            foreach( var piso in pisoExtra)
            {
                Destroy(piso);
            }
            pisoExtra.Clear();
            diccionarioObstaculos.Clear();
        }
    }
} 