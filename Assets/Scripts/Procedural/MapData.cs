using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.ChessMaze
{
    public struct MapData
    {
        public bool[] obstaclesArray;
        public Vector3 startPosition;
        public Vector3 exitPosition;
        public List<Vector3> path;
        public List<Vector3> cornerList;
        public int cornerNearEachOther;
        public Vector3 inicioRio;
        public Vector3 finalRio;
        public List<Vector3> rio;
        public List<Vector3> listaCurbasRio;
        public int cornerNearEachOtherRio;
    }
}
