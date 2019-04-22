using UnityEngine;

namespace MKdev.MqttForUnity
{

    public class MqttTopicToConsole : MonoBehaviour, IMqttTopicReceiver
    {
        public MqttConnector MqttCon;
        public string publishTopic = "hello/world";

        private void Start()
        {
            Invoke("SetupLate", 3f);
        }

        private void SetupLate()
        {
            MqttCon.AddTopicReceiver(publishTopic, this);
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            OnReceivedMessage(topic, System.Text.Encoding.UTF8.GetString(message));
        }

        public void OnReceivedMessage(string topic, string message)
        {
            Debug.Log("Topic: " + topic + " Message: " + message);
        }
    }
}