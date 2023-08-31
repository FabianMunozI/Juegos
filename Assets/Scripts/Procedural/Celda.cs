using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.ChessMaze
{
    public class Celda
    {
        private int x, z; // posiciones
        private bool isTaken;
        private TipoObjCelda tipoObjeto;

        public global::System.Int32 X { get => x; }
        public global::System.Int32 Z { get => z;  }
        public global::System.Boolean IsTaken { get => isTaken; set => isTaken = value; }
        public TipoObjCelda TipoObjeto { get => tipoObjeto; set => tipoObjeto = value; }


        public Celda(int x, int z)
        {
            this.x = x;
            this.z = z;
            this.tipoObjeto = TipoObjCelda.Empty;
            isTaken = false;
        }
    }

    public enum TipoObjCelda
    {
        Empty,
        Road,
        RoadPuente,
        Obstacle,
        Start,
        Exit,
        Rio,
        InicioRio,
        FinalRio
    }
}