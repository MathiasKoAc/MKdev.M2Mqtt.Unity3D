using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKdev.MqttForUnity
{
    public interface IMqttTopicReceiver
    {
        void OnReceivedMessage(string topic, byte[] message);
    }
}
