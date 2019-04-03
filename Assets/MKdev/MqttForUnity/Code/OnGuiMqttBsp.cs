using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MKdev.MqttForUnity;

public class OnGuiMqttBsp : MonoBehaviour {

    public MqttConnector MqttCon;
    public string publishTopic = "hello/world";
    public string publishString = "Sending from Unity3D!!!";

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 40, 80, 20), "publish now"))
        {
            MqttCon.PublishMessage(publishTopic, publishString);
        }
    }
}
