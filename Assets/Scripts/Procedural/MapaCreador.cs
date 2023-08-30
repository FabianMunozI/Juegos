using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;


namespace SVS.ChessMaze
{
    public class MapaCreador : MonoBehaviour
    {
        //X ANCHO
        //Z LARGO
        private GrillaMapa grilla;
        public int anchoMapa = 15, largoMapa = 15;
        private List<Vector3> listaObstaculos = null;
        private Vector3 inicioCamino, inicioRio, finalCamino, finalRio;
        private List<Vector3> camino = new List<Vector3>(), rio = new List<Vector3>();
        private List<Vector3> listaCurvasCamino = new List<Vector3>();
        public int largoRio = 15, alejateRio = 3, cantidadObstaculos;


        public GrillaMapa Grilla { get => grilla; }
        public List<Vector3> ListaObstaculos { get => listaObstaculos;}

        private Transform parent;
        public GameObject roadStraight, roadTileCorner, tileEmpty, startTile, exitTile;
        public GameObject[] enviromentTiles; // obstaculos 
        public GameObject rioRecto, curbaRio, puente, inRio, finRio;
        Dictionary<Vector3, GameObject> diccionarioObsts;
        List<GameObject> pisosExtra;

        public MapaCreador(GrillaMapa grilla)
        {
            this.grilla = grilla;
        }

        public void CrearNuevoMapa()
        {
            
            inicioCamino = SeleccionarInicioAleatoreo(grilla);
            camino = GenerarCamino(inicioCamino, ref listaCurvasCamino);

            // hay que setear las celdas de camino !!!! exepto principio y final

            inicioRio = SeleccionarInicioAleatoreo(grilla, camino, true);
            rio = GenerarRio(inicioRio, camino, listaCurvasCamino);

            //Ahora colocar obstaculos
            

            listaObstaculos = ColocarObstaculos();

            //visualizar mapa

            VisualizarMapa();


            UiController.instance.HideLoadScreen();
        }

        

        public List<Vector3> GenerarCamino(Vector3 inicio, ref List<Vector3> curvas)
        {
            int largoCamino = 25, verSiCurva = 0,sigueRecto = 0;
            Vector3 prueba, anterior = inicio;
            List<Vector3> camino = new List<Vector3>();
            List<Vector2> aVecinos = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };

            camino.Add(inicio);

            while (largoCamino > 0)
            {
                if (sigueRecto <= 0) 
                {
                    verSiCurva = 0;
                    foreach (var otro in aVecinos)
                    {
                        verSiCurva++;
                        prueba = anterior + new Vector3(otro.x, 0, otro.y);
                        if (grilla.IsCeldaValid(prueba.x, prueba.z) && prueba != anterior) // debo revisar que tampoco sea la celda anterior ni esté en el camino
                        {
                            camino.Add(prueba);
                            grilla.SetCelda(prueba.x, prueba.z, TipoObjCelda.Road, true);
                            anterior = prueba;
                            //reviso si es curva
                            if (camino.Count -3 >= 0) //ver que tenga al menos 3 elementos con el agregado o no se podra saber si es curva
                            {
                                if (Math.Abs(prueba.x - camino[camino.Count - 3].x) == 1 && Math.Abs(prueba.z - camino[camino.Count - 3].z) == 1)
                                {
                                    curvas.Add(prueba);
                                }
                            }
                            sigueRecto = 5; //puede salir puro recto PERO VAMOS A VER QUE SALE
                            break;
                        }
                    }
                }
                else
                {
                    if(sigueRecto <= 2) //dar la posibilidad de que no siempre siga recto la misma cantidad despues de unos bloques
                    {
                        int i = Random.Range(0, 2);
                        if (i == 0) // 1 perdio su oportunidad  0 no perdio su oportunidad
                        {
                            sigueRecto--;
                            continue;
                        }    
                    }
                    prueba = anterior + new Vector3(aVecinos[verSiCurva - 1].x, 0, aVecinos[verSiCurva - 1].y);
                    if (grilla.IsCeldaValid(prueba.x, prueba.z))
                    {
                        camino.Add(prueba);
                        anterior = prueba;
                    }
                    else
                    {
                        sigueRecto = 1;
                    }
                    
                    sigueRecto--;
                }
                largoCamino--;
            }

            grilla.SetCelda(camino[camino.Count -1 ].x, camino[camino.Count - 1].z, TipoObjCelda.FinalRio, true);

            return camino;
        }

        private List<Vector3> GenerarRio(Vector3 inicio, List<Vector3> camino, List<Vector3> curvas)
        {
            List<Vector3> rio = new List<Vector3>();
            int sigueRecto = 0,paraRecto = 0, largo = largoRio, alejate = 0; // alejateRio 3
            Vector3 prueba = inicio , anterior = inicio;
            List<Vector2> aVecinos = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };

            rio.Add(inicio);

            while (largo > 0)
            {
                if (prueba != anterior && paraRecto == 4) // los reviso todos y no hay vecinos posibles
                {
                    break;
                }
                if (sigueRecto <= 0 && alejate < 3) // puede seguir o girar
                {
                    paraRecto = 0; // me dice cual agrego para si seguir por ese camino 
                    foreach (var otro in aVecinos)
                    {
                        paraRecto++;
                        prueba = anterior + new Vector3(otro.x, 0, otro.y);
                        if (IsRioValid(prueba.x, prueba.z) && prueba != anterior && !curvas.Contains(prueba)) // revisar no sea la celda anterior ni sea curvaCamino
                        {
                            if (camino.Contains(prueba)) //si pertenece al camino ver si puedo incluirlo y hacerlo puente
                            {
                                if (alejate <= 0)
                                {
                                    alejate = 3;
                                    //rio.Add(prueba); // ver si lo agrego o no, porque no es solo rio
                                    grilla.SetCelda(prueba.x, prueba.z, TipoObjCelda.RoadPuente, true);
                                    anterior = prueba;
                                    break;
                                }
                            }
                            else if(!camino.Contains(prueba))
                            {
                                rio.Add(prueba);
                                grilla.SetCelda(prueba.x, prueba.z, TipoObjCelda.Rio, true);
                                anterior = prueba;
                                sigueRecto = 5;
                                break;
                            }
                            
                        }
                        
                    }
                }
                else // sigue recto
                {
                    prueba = anterior + new Vector3(aVecinos[paraRecto - 1].x, 0, aVecinos[paraRecto - 1].y);
                    if(IsRioValid(prueba.x, prueba.z) && !curvas.Contains(prueba))
                    {
                        if (camino.Contains(prueba) && alejate <= 0)
                        {
                            //rio.Add(prueba);
                            grilla.SetCelda(prueba.x, prueba.z, TipoObjCelda.RoadPuente, true);
                            alejate = 4;
                            anterior = prueba;
                        }
                        else if (!camino.Contains(prueba))
                        {
                            rio.Add(prueba);
                            grilla.SetCelda(prueba.x, prueba.z, TipoObjCelda.Rio, true);
                            anterior = prueba;
                        }
                        else
                        {
                            sigueRecto = 1;
                            alejateRio = 1;
                        }   
                    }
                    else
                    {
                        sigueRecto = 1;
                    }
                    sigueRecto--;
                    alejate--;
                }
                largo--;
            }

            grilla.SetCelda(camino[camino.Count - 1].x, camino[camino.Count - 1].z, TipoObjCelda.FinalRio, true);

            return rio;
        }


        private List<Vector3> ColocarObstaculos()
        {
            List<Vector3> obstaculos = new List<Vector3>();
            cantidadObstaculos = (grilla.Ancho * grilla.Largo) / 5;
            int index;

            for(int i = 0; i < cantidadObstaculos; i++)
            {
                index = Random.Range(0, grilla.Ancho * grilla.Largo);
                var celda = grilla.CalcularIndxCoordns(index);
                if (grilla.GetCelda(celda.x, celda.z).IsTaken == false)
                {
                    obstaculos.Add(celda);
                    grilla.SetCelda(celda.x, celda.z, TipoObjCelda.Obstacle, true);
                }
                else
                {
                    i--;
                }
            }

            return obstaculos;
        }


        public Vector3 SeleccionarInicioAleatoreo(GrillaMapa grilla, List<Vector3> camino = default(List<Vector3>), bool esRio = false)
        {
            Vector3 inicio = new Vector3();
            Direccion direccion = (Direccion)Random.Range(1, 5);

            if (!esRio)
            {
                switch (direccion)
                {
                    case Direccion.Derecha:
                        inicio = new Vector3(grilla.Ancho - 1, 0, Random.Range(0, grilla.Largo));
                        break;
                    case Direccion.Izquierda:
                        inicio = new Vector3(0, 0, Random.Range(0, grilla.Largo));
                        break;
                    case Direccion.Arriba:
                        inicio = new Vector3(Random.Range(0, grilla.Largo), 0, grilla.Largo - 1);
                        break;
                    case Direccion.Abajo:
                        inicio = new Vector3(Random.Range(0, grilla.Largo), 0, 0);
                        break;
                    default:
                        break;
                }

                grilla.SetCelda(inicio.x, inicio.z, TipoObjCelda.Start, true);
            }
            else
            {
                //me gusatria hacer que se aleje uno pero no sé si es mucho casho
                switch (direccion)
                {
                    case Direccion.Derecha:
                        do
                        {
                            inicio = new Vector3(grilla.Ancho - 4, 0, Random.Range(3, grilla.Largo - 3));
                        } while (camino.Contains(inicio));
                        break;
                    case Direccion.Izquierda:
                        do
                        {
                            inicio = new Vector3(3, 0, Random.Range(3, grilla.Largo - 3));
                        } while (camino.Contains(inicio));
                        break;
                    case Direccion.Arriba:
                        do
                        {
                            inicio = new Vector3(Random.Range(3, grilla.Largo - 3), 0, grilla.Largo - 4);
                        } while (camino.Contains(inicio));
                        break;
                    case Direccion.Abajo:
                        do
                        {
                            inicio = new Vector3(Random.Range(3, grilla.Largo - 3), 0, 3);
                        } while (camino.Contains(inicio));
                        break;
                    default:
                        break;
                }

                grilla.SetCelda(inicio.x, inicio.z, TipoObjCelda.InicioRio, true);
            }

            return inicio;
        }

        

        private bool IsRioValid(float x, float z)
        {
            if (x >= grilla.Ancho - 3 || x < 3 || z >= grilla.Largo - 3 || z < 3)
            {
                return false; //fuera del rango
            }
            return true;
        }

        private void VisualizarMapa()
        {
            for (int col = 0; col < grilla.Ancho; col++)
            {
                for (int fila = 0; fila < grilla.Largo ; fila++)
                {
                    var celda = grilla.GetCelda(col, fila);
                    var position = new Vector3(celda.X, 0, celda.Z);

                    var index = grilla.CalcularIndxCoordns(position.x, position.z);
              
                    Direccion direccionPrevia = Direccion.None;
                    Direccion direccionNext = Direccion.None;

                    //RIO
                    Direccion direccionPreviaRio = Direccion.None;
                    Direccion direccionNextRio = Direccion.None;

                    switch (celda.TipoObjeto)
                    {
                        case TipoObjCelda.Empty:
                            CrearIndicador(position, tileEmpty);
                            break;
                        case TipoObjCelda.Road:
                            //CrearIndicador(position,roadStraight);

                            if (camino.Count > 0)
                            {
                                direccionPrevia = GetDireccionCeldaPrevia(position);
                                direccionNext = GetDireccionCeldaNext(position);
                            }
                            if (direccionPrevia == Direccion.Arriba && direccionNext == Direccion.Derecha || direccionPrevia == Direccion.Derecha && direccionNext == Direccion.Arriba)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position, roadTileCorner, Quaternion.Euler(0, 180, 0));
                            }
                            else if (direccionPrevia == Direccion.Derecha && direccionNext == Direccion.Abajo || direccionPrevia == Direccion.Abajo && direccionNext == Direccion.Derecha)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position, roadTileCorner, Quaternion.Euler(0, 270, 0));
                            }
                            else if (direccionPrevia == Direccion.Abajo && direccionNext == Direccion.Izquierda || direccionPrevia == Direccion.Izquierda && direccionNext == Direccion.Abajo)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position, roadTileCorner);
                            }
                            else if (direccionPrevia == Direccion.Izquierda && direccionNext == Direccion.Arriba || direccionPrevia == Direccion.Arriba && direccionNext == Direccion.Izquierda)
                            {
                                position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position, roadTileCorner, Quaternion.Euler(0, 90, 0));
                            }
                            else if (direccionPrevia == Direccion.Derecha && direccionNext == Direccion.Izquierda || direccionPrevia == Direccion.Izquierda && direccionNext == Direccion.Derecha)
                            {
                                var poner2 = position + new Vector3(.4801336f, .56981165f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                rotacion2 = Quaternion.Euler(0, 90, 0);
                                var elemento2 = Instantiate(roadStraight, poner2, rotacion2);
                                elemento2.transform.parent = parent;
                                //pisosExtra.Add(elemento2); //! qué pasa si la borro ?
                                diccionarioObsts.Add(position, elemento2);
                            }
                            else
                            {
                                var poner2 = position + new Vector3(.4801336f, .5446233f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                var elemento2 = Instantiate(roadStraight, poner2, rotacion2);
                                elemento2.transform.parent = parent;
                                //pisosExtra.Add(elemento2);
                                diccionarioObsts.Add(position, elemento2);
                            }


                            break;
                        case TipoObjCelda.Obstacle:
                            int randomIndex = Random.Range(0, enviromentTiles.Length);

                            var poner = position + new Vector3(.5f, .5f, .5f);
                            Quaternion rotacion = new Quaternion();
                            var elemento = Instantiate(tileEmpty, poner, rotacion);
                            elemento.transform.parent = parent;
                            pisosExtra.Add(elemento);

                            CrearIndicador(position, enviromentTiles[randomIndex]);
                            break;
                        case TipoObjCelda.Start:
                            if (camino.Count > 0)
                            {
                                direccionNext = GetDireccionVector(camino[0], position);
                            }
                            if (direccionNext == Direccion.Derecha || direccionNext == Direccion.Izquierda)
                            {
                                CrearIndicador(position, startTile, Quaternion.Euler(0, 90, 0));
                            }
                            else
                            {
                                CrearIndicador(position, startTile);
                            }
                            break;
                        case TipoObjCelda.Exit:
                            position = position + new Vector3(0, .01f, 0);
                            if (camino.Count > 0)
                            {
                                direccionPrevia = GetDireccionCeldaPrevia(position);
                            }
                            switch (direccionPrevia)
                            {
                                case Direccion.Derecha:
                                    CrearIndicador(position, exitTile, Quaternion.Euler(0, 90, 0));
                                    break;
                                case Direccion.Izquierda:
                                    CrearIndicador(position, exitTile, Quaternion.Euler(0, -90, 0));
                                    break;
                                case Direccion.Abajo:
                                    CrearIndicador(position, exitTile, Quaternion.Euler(0, 180, 0));
                                    break;
                                default:
                                    CrearIndicador(position, exitTile);
                                    break;
                            }
                            break;

                        //RIO    
                        case TipoObjCelda.RoadPuente:

                            if (camino.Count > 0)
                            {
                                direccionPreviaRio = GetDireccionCeldaPreviaRio(position);
                                direccionNextRio = GetDireccionCeldaNextRio(position);
                            }

                            if (direccionPreviaRio == Direccion.Derecha || direccionNextRio == Direccion.Izquierda || direccionPreviaRio == Direccion.Izquierda || direccionNextRio == Direccion.Derecha)
                            {
                                var posPuente = position + new Vector3(.5f, .5f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                var elemento2 = Instantiate(puente, posPuente, rotacion2);
                                elemento2.transform.parent = parent;

                                //Rio abajo de puente
                                Quaternion rotacionRio = Quaternion.Euler(0, 90, 0);
                                var rioPuente = Instantiate(rioRecto, posPuente, rotacionRio);
                                rioPuente.transform.parent = parent;
                                pisosExtra.Add(rioPuente);

                                diccionarioObsts.Add(position, elemento2);
                            }
                            else
                            {
                                var posPuente = position + new Vector3(.5f, .5f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                rotacion2 = Quaternion.Euler(0, 90, 0);
                                var elemento2 = Instantiate(puente, posPuente, rotacion2);
                                elemento2.transform.parent = parent;

                                //Rio abajo de puente
                                Quaternion rotacionRio = new Quaternion();
                                var rioPuente = Instantiate(rioRecto, posPuente, rotacionRio);
                                rioPuente.transform.parent = parent;
                                pisosExtra.Add(rioPuente);

                                diccionarioObsts.Add(position, elemento2);
                            }

                            break;
                        case TipoObjCelda.Rio:

                            if (rio.Count > 0)
                            {
                                direccionPreviaRio = GetDireccionCeldaPreviaRio(position);
                                direccionNextRio = GetDireccionCeldaNextRio(position);
                            }
                            if (direccionPreviaRio == Direccion.Arriba && direccionNextRio == Direccion.Derecha || direccionPreviaRio == Direccion.Derecha && direccionNextRio == Direccion.Arriba)
                            {
                                //position = position + new Vector3(0, -0.077f, 0);
                                CrearIndicador(position, curbaRio, Quaternion.Euler(0, 180, 0));
                            }
                            else if (direccionPreviaRio == Direccion.Derecha && direccionNextRio == Direccion.Abajo || direccionPreviaRio == Direccion.Abajo && direccionNextRio == Direccion.Derecha)
                            {
                                CrearIndicador(position, curbaRio, Quaternion.Euler(0, 270, 0));
                            }
                            else if (direccionPreviaRio == Direccion.Abajo && direccionNextRio == Direccion.Izquierda || direccionPreviaRio == Direccion.Izquierda && direccionNextRio == Direccion.Abajo)
                            {
                                CrearIndicador(position, curbaRio);
                            }
                            else if (direccionPreviaRio == Direccion.Izquierda && direccionNextRio == Direccion.Arriba || direccionPreviaRio == Direccion.Arriba && direccionNextRio == Direccion.Izquierda)
                            {
                                CrearIndicador(position, curbaRio, Quaternion.Euler(0, 90, 0));
                            }
                            else if (direccionPreviaRio == Direccion.Derecha && direccionNextRio == Direccion.Izquierda || direccionPreviaRio == Direccion.Izquierda && direccionNextRio == Direccion.Derecha)
                            {
                                position = position + new Vector3(.5f, 0.5f, .5f); //! comentar si no sale
                                Quaternion rotacion2 = new Quaternion();
                                rotacion2 = Quaternion.Euler(0, 90, 0);
                                //var elemento2 = Instantiate(rioRecto, poner2, rotacion2);
                                var elemento2 = Instantiate(rioRecto, position, rotacion2, parent);
                                //elemento2.transform.parent = parent;
                                diccionarioObsts.Add(position, elemento2);
                            }
                            else
                            {
                                position = position + new Vector3(.5f, 0.5f, .5f);
                                Quaternion rotacion2 = new Quaternion();
                                //var elemento2 = Instantiate(rioRecto, poner2, rotacion2);
                                var elemento2 = Instantiate(rioRecto, position, rotacion2, parent);
                                //elemento2.transform.parent = parent;
                                diccionarioObsts.Add(position, elemento2);
                            }

                            break;
                        case TipoObjCelda.InicioRio:

                            if (rio.Count > 0)
                            {
                                direccionNextRio = GetDireccionVector(rio[0], position);
                            }

                            switch (direccionNextRio)
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

                            if (rio.Count > 0)
                            {
                                direccionPreviaRio = GetDireccionCeldaPreviaRio(position);
                            }
                            switch (direccionPreviaRio)
                            {
                                case (Direccion.Derecha):
                                    CrearIndicador(position, finRio, Quaternion.Euler(0, 270, 0));
                                    break;
                                case (Direccion.Izquierda):
                                    CrearIndicador(position, finRio, Quaternion.Euler(0, 90, 0));
                                    break;
                                case (Direccion.Abajo):
                                    CrearIndicador(position, finRio);
                                    break;
                                default:
                                    CrearIndicador(position, finRio, Quaternion.Euler(0, 180, 0));
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        private Direccion GetDireccionCeldaNext(Vector3 position)
        {
            int index = camino.FindIndex(async => async == position);
            var nextCellPosition = camino[index + 1]; //sabemos que existe index + 1 
            return GetDireccionVector(nextCellPosition, position);
        }

        private Direccion GetDireccionCeldaNextRio(Vector3 position)
        {
            int index = rio.FindIndex(async => async == position);
            var nextCellPosition = rio[index + 1]; //sabemos que existe index + 1 
            return GetDireccionVector(nextCellPosition, position);
        }

        private Direccion GetDireccionCeldaPrevia(Vector3 position)
        {
            var index = camino.FindIndex(async => async == position);
            var previaCeldaPos = Vector3.zero;
            if (index > 0) // no es la primera posicion en el path
            {
                previaCeldaPos = camino[index - 1];
            }
            else
            {
                previaCeldaPos = inicioCamino;
            }
            return GetDireccionVector(previaCeldaPos, position);
        }

        private Direccion GetDireccionCeldaPreviaRio(Vector3 position)
        {
            var index = rio.FindIndex(async => async == position);
            var previaCeldaPos = Vector3.zero;
            if (index > 0) // no es la primera posicion en el path
            {
                previaCeldaPos = rio[index - 1];
            }
            else
            {
                previaCeldaPos = inicioRio;
            }
            return GetDireccionVector(previaCeldaPos, position);
        }

        private Direccion GetDireccionVector(Vector3 positionGo, Vector3 position)
        {
            if (positionGo.x > position.x)
            {
                return Direccion.Derecha;
            }
            else if (positionGo.x < position.x)
            {
                return Direccion.Izquierda;
            }
            else if (positionGo.z < position.z)
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
            diccionarioObsts.Add(position, element);
        }

        public void LimpiarMapita()
        {
            Debug.Log("aca");
            Debug.Log(diccionarioObsts);
            foreach (var obstacle in diccionarioObsts.Values)
            {
                Debug.Log("entre");
                Destroy(obstacle);
            }
            foreach (var piso in pisosExtra)
            {
                Destroy(piso);
            }
            pisosExtra.Clear();
            diccionarioObsts.Clear();
        }

        public void CorrerAlgoritmo()
        {

            UiController.instance.ResetScreen();

            //ResetVariables();
            Debug.Log("antes");
            Debug.Log(diccionarioObsts);
            LimpiarMapita();

            grilla = new GrillaMapa(anchoMapa, largoMapa);

            CrearNuevoMapa();
        }


        /*
        private void ResetVariables()
        {
            this.grilla = grilla;
        public int anchoMapa = 15, largoMapa = 15;
        private List<Vector3> listaObstaculos = null;
        private Vector3 inicioCamino, inicioRio, finalCamino, finalRio;
        private List<Vector3> camino = new List<Vector3>(), rio = new List<Vector3>();
        private List<Vector3> listaCurvasCamino = new List<Vector3>();
        public int largoRio = 15, alejateRio = 3, cantidadObstaculos;


        public GrillaMapa Grilla { get => grilla; }
        public List<Vector3> ListaObstaculos { get => listaObstaculos; }

        private Transform parent;
        public GameObject roadStraight, roadTileCorner, tileEmpty, startTile, exitTile;
        public GameObject[] enviromentTiles; // obstaculos 
        public GameObject rioRecto, curbaRio, puente, inRio, finRio;
        Dictionary<Vector3, GameObject> diccionarioObsts = new Dictionary<Vector3, GameObject>();
        List<GameObject> pisosExtra = new List<GameObject>();
        }
        */

        
        void Start()
        {
            diccionarioObsts = new Dictionary<Vector3, GameObject>();
            pisosExtra = new List<GameObject>();
            Debug.Log("start");
            Debug.Log(diccionarioObsts);
        }
        

    }
}

