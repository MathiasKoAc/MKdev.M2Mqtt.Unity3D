using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MKdev.ServerConfig.ConfigEditor
{
    [CustomEditor(typeof(ConfigFromJSONFile))]
    public class ConfigFromFileEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ConfigFromJSONFile myScript = (ConfigFromJSONFile)target;

            if (myScript.filePath == "")
            {
                EditorGUILayout.HelpBox("Please Setup File Path", MessageType.Warning);
            }
            
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