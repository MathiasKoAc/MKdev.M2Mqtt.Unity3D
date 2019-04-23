using UnityEngine;

namespace MKdev.MqttForUnity.MqttBricks
{
    /**
     * It is a Brick, which does a Action if the configert Topic changes a value
     */
    public class DoActionByMqttTopicArives : DoActionByMqttTopicValue
    {
        public override bool compareOk(byte[] message)
        {
            return true;
        }
    }

}

