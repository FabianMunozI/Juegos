using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SVS.AI
{
    public class VertexPositionRio : IEquatable<VertexPositionRio>, IComparable<VertexPositionRio>
    {
        public static List<Vector2Int> posiblesVecinos = new List<Vector2Int>
        {
            new Vector2Int(0,-1),
            new Vector2Int(0,1),
            new Vector2Int(1,0),
            new Vector2Int(-1,0)
        };

        public float costoTotal, costoEstimado;
        public VertexPositionRio previousVertex = null;
        public VertexPositionRio exVertex = null;
        private Vector3 position;
        private bool isTaken;

        public int X { get => (int)position.x; }
        public int Z { get => (int)position.z; }

        public Vector3 Position {get => position;}
        public global::System.Boolean IsTaken { get => isTaken;}

        public VertexPositionRio(Vector3 position, bool isTaken = false)
        {
            this.position = position;
            this.isTaken = isTaken;
            this.costoEstimado  = 0;
            this.costoTotal = 1;
        }

        public int GetHashCode(VertexPositionRio obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

        public bool Equals(VertexPositionRio other)
        {
            return Position == other.Position;
        }

        public int CompareTo(VertexPositionRio other)
        {
            if (this.costoEstimado < other.costoEstimado) return -1;
            if (this.costoEstimado > other.costoEstimado) return 1;
            return 0;
        }
    }
}