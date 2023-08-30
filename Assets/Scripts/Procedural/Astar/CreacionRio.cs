using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVS.ChessMaze;
using System.Linq;
using Random = UnityEngine.Random;

namespace SVS.AI
{
    public static class CreacionRio
    {
        public static List<Vector3> GetRio(Vector3 start, bool[] obstArray, GrillaMapa grilla, List<Vector3> path, List<Vector3> curbasCamino)
        {
            List<Vector3> rio = new List<Vector3>();
            Vector3 siguiente;

            start = CalcularInicio(path, grilla, obstArray);

            var prueba = start;

            int largoRio = 15;
            //int recto = 5;
            int alejate = 0;

            while (largoRio > 0)
            {
                siguiente = SiguienteValido(prueba, grilla, obstArray);

                if (siguiente != new Vector3(grilla.Largo, 0, grilla.Ancho))
                {
                    if (!path.Contains(siguiente))
                    {
                        rio.Add(siguiente);
                        prueba = siguiente;
                    }
                    else
                    {
                        if(alejate <= 0 && !curbasCamino.Contains(siguiente))
                        {
                            rio.Add(siguiente);
                            alejate = 3;
                            prueba = siguiente;
                        }
                        else
                        {
                            //buscar otro camino , puede que quedaran vecinos sin revisar
                            break;
                        }
                    }
                    
                }
                else
                {
                    break;
                }
                alejate--;
                largoRio--;
            }
            return rio;
        }

        private static Vector3 CalcularInicio(List<Vector3> path, GrillaMapa grilla, bool[] obstArray)
        {
            //revisar que el inicio del rio esté lejos del camino
            Vector3 dif1 = new Vector3(1, 0, 0);
            Vector3 dif2 = new Vector3(0, 0, 1);
            Vector3 start;

            do
            {
                do
                {
                    start = new Vector3(Random.Range(4, grilla.Largo - 3), 0, Random.Range(4, grilla.Ancho - 3));
                    Debug.Log("start" + start);
                } while (path.Contains(start) || path.Contains(start + dif1) || path.Contains(start - dif1) || path.Contains(start + dif2) || path.Contains(start - dif2));
            } while (!EsValida(start, grilla, obstArray));
    
        return start;
        }


       

        private static Vector3 SiguienteValido(Vector3 actual, GrillaMapa grilla, bool[] obstArray)
        {
            List<Vector3> posibles = new List<Vector3> { new Vector3(0, 0, -1), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };
            Vector3 prueba, siguiente = new Vector3(grilla.Largo, 0, grilla.Ancho);

            foreach (var i in posibles)
            {
                prueba = actual + i;
                if (prueba.x < 3 || prueba.x > grilla.Largo - 3 || prueba.z < 3 || prueba.z > grilla.Ancho - 3 || !EsValida(prueba, grilla, obstArray))
                {
                    continue;
                }
                else
                {
                    siguiente = prueba;
                }
            }
            return siguiente;
        }

        private static bool EsValida(Vector3 posicion, GrillaMapa grilla, bool[] obstArray)
        {
            return obstArray[grilla.CalcularIndxCoordns(posicion.x, posicion.z)];
        }
    }
}

