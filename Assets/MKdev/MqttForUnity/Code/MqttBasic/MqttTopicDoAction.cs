using UnityEngine;
using UnityEngine.Events;

namespace MKdev.MqttForUnity
{
    public class MqttTopicDoAction : MonoBehaviour, IMqttTopicReceiver
    {

        public MqttConnector MqttCon;
        public string publishTopic = "hello/world";

        public InteractionEvent onStart = new InteractionEvent();

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
            onStart.Invoke();
        }
    }
}

