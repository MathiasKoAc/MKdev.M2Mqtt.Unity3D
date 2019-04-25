using System;
using MKdev.Logic;

namespace MKdev.MqttForUnity.MqttBricks
{
    public class DoActionByMqttTopicStringValue : AbsDoActionByMqttTopicValue
    {
        public EnumStringCompareOperator Compare;
        public string StrValue;

        public override bool compareOk(byte[] message)
        {
            if (message.Length > 0)
            {
                string topicVal = BitConverter.ToString(message);

                switch (Compare)
                {
                    case EnumStringCompareOperator.equal:
                        return topicVal.Equals(StrValue);
                    case EnumStringCompareOperator.equalIgnoreCase:
                        return topicVal.Equals(StrValue, StringComparison.OrdinalIgnoreCase);
                    case EnumStringCompareOperator.notEqual:
                        return !topicVal.Equals(StrValue);
                    case EnumStringCompareOperator.notEqualIgnoreCase:
                        return !topicVal.Equals(StrValue, StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }
    }
}
