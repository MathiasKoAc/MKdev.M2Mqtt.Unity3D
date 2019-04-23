using MKdev.Logic;
using System;

namespace MKdev.MqttForUnity.MqttBricks
{
    public class DoActionByMqttTopicIntValue : DoActionByMqttTopicValue
    {
        public EnumCompareOperator Compare;
        public int IntValue;

        public override bool compareOk(byte[] message)
        {
            if(message.Length > 0)
            {
                int topicVal = BitConverter.ToInt32(message, 0);

                switch (Compare)
                {
                    case EnumCompareOperator.bigger:
                        return topicVal > IntValue;
                    case EnumCompareOperator.biggerOr:
                        return topicVal >= IntValue;
                    case EnumCompareOperator.equal:
                        return topicVal == IntValue;
                    case EnumCompareOperator.smaller:
                        return topicVal < IntValue;
                    case EnumCompareOperator.smallerOr:
                        return topicVal <= IntValue;
                }
            }

            return false;
        }
    }
}
