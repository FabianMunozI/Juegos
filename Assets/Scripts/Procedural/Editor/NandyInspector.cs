using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SVS.ChessMaze
{
    [CustomEditor(typeof(MapaCreador))]

    public class NandyInspector : Editor
    {
        MapaCreador mapaCreador;

        private void OnEnable()
        {
            mapaCreador = (MapaCreador)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                //GUI.enabled = !mapaCreador.IsAlgRun; // quizas esto no lo necesite
                if (GUILayout.Button("Crear Mapita"))
                {
                    mapaCreador.CorrerAlgoritmo();
                }
            }
        }
    }
}