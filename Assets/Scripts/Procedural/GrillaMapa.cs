using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SVS.ChessMaze
{
    public class GrillaMapa
    {
        private int ancho, largo;
        private Celda[,] grillaCeldas;

        public global::System.Int32 Ancho { get => ancho; }
        public global::System.Int32 Largo { get => largo;  }

        public GrillaMapa(int ancho, int largo)
        {
            this.ancho = ancho;
            this.largo = largo;
            CrearGrilla();
        }

        private void CrearGrilla()
        {
            grillaCeldas = new Celda[largo, ancho];
            for ( int fila = 0 ; fila < largo ; fila++ )
            {
                for ( int colum = 0 ; colum < ancho ; colum++ )
                {
                    grillaCeldas[fila,colum] = new Celda(colum,fila);
                }
            }
        }

        //metodos auxiliares

        public void SetCelda(int x, int z, TipoObjCelda tipoObjeto, bool isTake = false)
        {
            grillaCeldas[z,x].TipoObjeto = tipoObjeto;
            grillaCeldas[z,x].IsTaken = isTake;
        }

        public void SetCelda(float x, float z, TipoObjCelda tipoObjeto, bool isTaken = false)
        {
            SetCelda((int)x, (int)z,tipoObjeto,isTaken);
        }

        public void UnsetCelda(int x, int z)
        {
            grillaCeldas[z, x].TipoObjeto = TipoObjCelda.Empty;
            grillaCeldas[z, x].IsTaken = false;
        }
        public TipoObjCelda GetCeldaType(int x, int z)
        {
            return grillaCeldas[z, x].TipoObjeto;
        }

        public bool IsCeldaTaken(int x, int z)
        {
            return grillaCeldas[z,x].IsTaken;
        }

        public bool IsCeldaTaken(float x, float z)
        {
            return grillaCeldas[ (int)z , (int)x ].IsTaken;
        }

        public int CalcularIndxCoordns(int x, int z)
        {
            return x + z * ancho;
        }

        public int CalcularIndxCoordns(float x, float z)
        {
            return (int)x + (int)z * ancho;
        }

        public Vector3 CalcularIndxCoordns(int randomIndex)
        {
            int x = randomIndex % ancho;
            int z = randomIndex / ancho;
            return new Vector3( x,0,z);
        }

        public bool IsCeldaValid(float x, float z)
        {
            if ( x >= ancho || x < 0 || z >= largo || z < 0 )
            {
                return false; //fuera del rango
            }
            return true;
        }

        public Celda GetCelda(int x, int z)
        {
            if ( IsCeldaValid(x,z) == false)
            {
                return null;
            }
            return grillaCeldas[z,x];
        }

        public Celda GetCelda(float x, float z)
        {
            return GetCelda((int)x, (int)z);
        }



        public void CheckCoordenadas()
        {
            for (int i = 0 ; i < grillaCeldas.GetLength(0) ; i++ )
            {
                StringBuilder b = new StringBuilder();
                for (int j = 0 ; j < grillaCeldas.GetLength(1) ; j++ )
                {
                    b.Append(j + "," + i + " ");
                }
                Debug.Log(b.ToString());
            }
        }
    }
}
