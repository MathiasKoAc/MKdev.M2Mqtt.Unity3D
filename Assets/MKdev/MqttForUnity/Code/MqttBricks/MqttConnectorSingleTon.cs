using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MKdev.MqttForUnity
{
    public class MqttConnectorSingleTon : MqttConnector
    {

        // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
        private static MqttConnectorSingleTon s_Instance = null;


        // A static property that finds or creates an instance of the manager object and returns it.
        public static MqttConnectorSingleTon GetInstance()
        {
            if (s_Instance == null)
            {
                // FindObjectOfType() returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(MqttConnectorSingleTon)) as MqttConnectorSingleTon;
            }

            if (s_Instance == null)
            {
                Debug.LogError("Error MqttConnectorTaggedSingle: Can not find a GameObject from Type MqttConnectorSingleTon");
            }

            return s_Instance;
        }


        // Ensure that the instance is destroyed when the game is stopped in the editor.
        void OnApplicationQuit()
        {
            s_Instance = null;
        }
    }
}

