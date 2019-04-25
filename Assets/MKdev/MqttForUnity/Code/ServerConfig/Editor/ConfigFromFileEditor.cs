using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MKdev.ServerConfig
{
    [CustomEditor(typeof(ConfigFromJSONFile))]
    public class ConfigFromFileEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ConfigFromJSONFile myScript = (ConfigFromJSONFile)target;
            if (GUILayout.Button("Write to File"))
            {
                if (myScript != null)
                {
                    myScript.WriteToFile();
                }
            }

            if (GUILayout.Button("Read from File"))
            {
                if (myScript != null)
                {
                    myScript.ReadFromFile();
                }
            }
        }
    }
}