using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SVS.ChessMaze
{
    [CustomEditor(typeof(MapBrain))]

    public class BrainInspector : Editor
    {
        MapBrain mapBrain;

        private void OnEnable()
        {
            mapBrain = (MapBrain)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                GUI.enabled = !mapBrain.IsAlgRun;
                if (GUILayout.Button("Correr Algortimo genético"))
                {
                    mapBrain.RunAlgoritmo();
                }
            }
        }
    }
}

