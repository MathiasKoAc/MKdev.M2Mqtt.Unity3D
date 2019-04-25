using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MKdev.MqttForUnity.MqttBricks
{
    [CustomEditor(typeof(AbsDoActionByMqttTopicValue))]
    public class AbsDoActionByMqttTopicValueEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AbsDoActionByMqttTopicValue myScript = (AbsDoActionByMqttTopicValue)target;

            if (GUILayout.Button("Do onStart NOW"))
            {
                if (myScript != null)
                {
                    myScript.onStart.Invoke();
                }
            }
        }
    }
}