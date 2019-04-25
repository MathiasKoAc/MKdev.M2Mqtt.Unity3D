using UnityEngine;

namespace MKdev.ServerConfig
{
    [CreateAssetMenu(fileName = "Data", menuName = "MKDev/SeverConfig/ConfigObject", order = 1)]
    public class ConfigObject : AbsServerConfig
    {
        public string ServerIp = "192.168.178.2";
        public string Username = "uname";
        public string Password = "securePwd";

        public override string GetServerIp()
        {
            return this.ServerIp;
        }

        public override string GetUsername()
        {
            return this.Username;
        }

        public override string GetPassword()
        {
            return this.Password;
        }
    }

}
