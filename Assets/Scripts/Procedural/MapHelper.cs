using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SVS.ChessMaze
{
    public static class MapHelper
    {
        public static void RandomChooseStartExit(GrillaMapa grilla, ref Vector3 startPosition, ref Vector3 exitPosition, Vector3 inCamino = default(Vector3), Vector3 finCamino = default(Vector3), bool rios = false)
        {
            if (rios)
            {
                startPosition = RandomBorde(grilla, startPosition, inCamino, true);
                exitPosition = RandomBorde(grilla, startPosition, finCamino, true);

                Debug.Log("Rio: ");
                Debug.Log("start: { " + startPosition.x + " , " + startPosition.y + " , " + startPosition.z + " }");
                Debug.Log("end  : { " + exitPosition.x + " , " + exitPosition.y + " , " + exitPosition.z + " }");

                grilla.SetCelda(startPosition.x, startPosition.z, TipoObjCelda.InicioRio, true);
                grilla.SetCelda(exitPosition.x, exitPosition.z, TipoObjCelda.FinalRio, true);
            }
            else
            {
                startPosition = RandomBorde(grilla, startPosition);
                exitPosition = RandomBorde(grilla, startPosition);

                Debug.Log("Camino: ");
                Debug.Log("start: { " + startPosition.x + " , " + startPosition.y + " , " + startPosition.z + " }");
                Debug.Log("end  : { " + exitPosition.x + " , " + exitPosition.y + " , " + exitPosition.z + " }");

                grilla.SetCelda(startPosition.x, startPosition.z, TipoObjCelda.Start, true);
                grilla.SetCelda(exitPosition.x, exitPosition.z, TipoObjCelda.Exit, true);
            }
        }

        public static Vector3 RandomBorde(GrillaMapa grilla, Vector3 startPosition, Vector3 camino = default(Vector3), bool a = false)
        {
            Direccion direccion = (Direccion)Random.Range(1, 5);

            Vector3 position = Vector3.zero;

            if (a)
            {
                do
                {
                    switch (direccion)
                    {
                        case Direccion.Derecha:
                            do
                            {
                                position = new Vector3(grilla.Ancho - 4, 0, Random.Range(0, grilla.Largo - 3));
                            } while (Vector3.Distance(position, startPosition) <= 1);
                            break;
                        case Direccion.Izquierda:
                            do
                            {
                                position = new Vector3(3, 0, Random.Range(0, grilla.Largo - 3));
                            } while (Vector3.Distance(position, startPosition) <= 1);
                            break;
                        case Direccion.Arriba:
                            do
                            {
                                position = new Vector3(Random.Range(0, grilla.Largo - 3), 0, grilla.Largo - 4);
                            } while (Vector3.Distance(position, startPosition) <= 1);
                            break;
                        case Direccion.Abajo:
                            do
                            {
                                position = new Vector3(Random.Range(0, grilla.Largo - 3), 0, 3);
                            } while (Vector3.Distance(position, startPosition) <= 1);
                            break;
                        default:
                            break;
                    }
                } while (position == camino);
            }
            else
            {
                switch (direccion)
                {
                    case Direccion.Derecha:
                        do
                        {
                            position = new Vector3(grilla.Ancho - 1, 0, Random.Range(0, grilla.Largo));
                        } while (Vector3.Distance(position, startPosition) <= 1);
                        break;
                    case Direccion.Izquierda:
                        do
                        {
                            position = new Vector3(0, 0, Random.Range(0, grilla.Largo));
                        } while (Vector3.Distance(position, startPosition) <= 1);
                        break;
                    case Direccion.Arriba:
                        do
                        {
                            position = new Vector3(Random.Range(0, grilla.Largo), 0, grilla.Largo - 1);
                        } while (Vector3.Distance(position, startPosition) <= 1);
                        break;
                    case Direccion.Abajo:
                        do
                        {
                            position = new Vector3(Random.Range(0, grilla.Largo), 0, 0);
                        } while (Vector3.Distance(position, startPosition) <= 1);
                        break;
                    default:
                        break;
                }
            }
            return position;
        }
    }
}

