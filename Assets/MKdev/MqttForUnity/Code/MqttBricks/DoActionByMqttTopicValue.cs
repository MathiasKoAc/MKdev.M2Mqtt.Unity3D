using UnityEngine;

namespace MKdev.MqttForUnity.MqttBricks
{
    public abstract class DoActionByMqttTopicValue : MonoBehaviour, IMqttTopicReceiver
    {

        private MqttConnector MqttCon;

        public InteractionEvent onStart;

        public string publishTopic;

        private void Start()
        {
            Invoke("SetupLate", 3f);
        }

        private void SetupLate()
        {
            MqttCon = MqttConnectorSingleTon.GetInstance();
            MqttCon.AddTopicReceiver(publishTopic, this);
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            if(compareOk(message))
            {
                onStart.Invoke();
            }
        }

        public abstract bool compareOk(byte[] message);
    }

}

