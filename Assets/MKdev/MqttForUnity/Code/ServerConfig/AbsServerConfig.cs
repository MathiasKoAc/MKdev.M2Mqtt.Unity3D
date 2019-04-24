using UnityEngine;

namespace MKdev.ServerConfig
{
    public abstract class AbsServerConfig : ScriptableObject
    {
        public abstract string GetServerIp();

        public abstract string GetPassword();

        public abstract string GetUsername();
    }
}