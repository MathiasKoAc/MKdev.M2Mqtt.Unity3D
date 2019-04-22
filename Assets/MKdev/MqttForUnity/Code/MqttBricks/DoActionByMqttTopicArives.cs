using UnityEngine;

namespace MKdev.MqttForUnity
{
    public class DoActionByMqttTopicArives : DoActionByMqttTopicValue
    {
        public override bool compareOk(byte[] message)
        {
            return true;
        }
    }

}

