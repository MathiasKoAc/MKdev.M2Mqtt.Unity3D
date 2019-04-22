using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKdev.MqttForUnity
{
    public enum EnumMqttQualityOfService : byte
    {
        QOS_LEVEL_AT_MOST_ONCE=0x00, 
        QOS_LEVEL_AT_LEAST_ONCE = 0x01,
        QOS_LEVEL_EXACTLY_ONCE = 0x02
    }
}
