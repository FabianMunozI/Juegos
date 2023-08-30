using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVS.ChessMaze;

namespace SVS.AI
{
    public static class Astar
    {
        public static List<Vector3> GetPath(Vector3 start, Vector3 exit, bool[] obstArray, GrillaMapa grilla)
        {
            VertexPosition startVertex = new VertexPosition(start);
            VertexPosition exitVertex = new VertexPosition(exit);

            List<Vector3> path = new List<Vector3>();

            List<VertexPosition> openedList = new List<VertexPosition>();
            HashSet<VertexPosition> closedList = new HashSet<VertexPosition>();

            startVertex.costoEstimado = ManhatanDistance(startVertex,exitVertex);

            openedList.Add(startVertex);

            VertexPosition currentVertex = null;

            while ( openedList.Count > 0)
            {
                openedList.Sort();
                currentVertex = openedList[0];

                if (currentVertex.Equals(exitVertex))
                {
                    while ( currentVertex != startVertex)
                    {
                        path.Add(currentVertex.Position);
                        currentVertex = currentVertex.previousVertex;
                    }
                    path.Reverse();
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

            return path;
        }

        private static VertexPosition[] FindVecinos(VertexPosition currentVertex, GrillaMapa grilla, bool[] obstArray)
        {
            VertexPosition[] arrayVecinos = new VertexPosition[4]; //max 4 vecinos

            int arrayIndex = 0;
            foreach ( var posiblesVecinos in VertexPosition.posiblesVecinos)
            {
                Vector3 position = new Vector3(currentVertex.X + posiblesVecinos.x, 0, currentVertex.Z + posiblesVecinos.y);
                if (grilla.IsCeldaValid(position.x, position.z))
                {
                    int index = grilla.CalcularIndxCoordns(position.x, position.z);
                    arrayVecinos[arrayIndex] = new VertexPosition(position,obstArray[index]);
                    arrayIndex++;
                }
            }
            return arrayVecinos;
        }

        private static float ManhatanDistance(VertexPosition startVertex, VertexPosition exitVertex)
        {
            return Mathf.Abs(startVertex.X - exitVertex.X) + Mathf.Abs(startVertex.Z - exitVertex.Z);
        }
    }
}
