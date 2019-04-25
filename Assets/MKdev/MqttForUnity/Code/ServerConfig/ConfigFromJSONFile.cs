
using UnityEngine;

namespace MKdev.ServerConfig
{

    class ConfigDetail
    {

        public string ServerIp;
        public string Username;
        public string Password;

        public ConfigDetail()
        {
        }

        public ConfigDetail(string serverIp, string username, string password)
        {
            this.ServerIp = serverIp;
            this.Username = username;
            this.Password = password;
        }
    }

    [CreateAssetMenu(fileName = "Data", menuName = "MKDev/SeverConfig/ConfigFromJSONFile", order = 1)]
    public class ConfigFromJSONFile : AbsServerConfig
    {
        public string ServerIp = "192.168.178.2";
        public string Username = "uname";
        public string Password = "securePwd";

        public string filePath = "";
        public bool onEnableReadFromFile = false;

        public void WriteToFile()
        {
            string json = JsonUtility.ToJson(new ConfigDetail(this.ServerIp, this.Username, this.Password));

            //if (!System.IO.File.Exists(filePath))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(filePath))
                {
                    byte[] jsonInByte = System.Text.Encoding.UTF8.GetBytes(json);
                    fs.Write(jsonInByte, 0, jsonInByte.Length);
                }
            }
        }

        public void ReadFromFile()
        {
            if (System.IO.File.Exists(filePath))
            {
                string text = System.IO.File.ReadAllText(filePath);
                if (text.Length > 1)
                {
                    ConfigDetail detail = JsonUtility.FromJson<ConfigDetail>(text);
                    this.ServerIp = detail.ServerIp;
                    this.Username = detail.Username;
                    this.Password = detail.Password;
                }
                else
                {
                    Debug.LogError("Error ConfigFromFile: Can't find File: " + filePath);
                }
            }
        }

        public void OnEnable()
        {
            if(onEnableReadFromFile)
            {
                ReadFromFile();
            }
            
        }

        public override string GetServerIp()
        {
            throw new System.NotImplementedException();
        }

        public override string GetPassword()
        {
            throw new System.NotImplementedException();
        }

        public override string GetUsername()
        {
            throw new System.NotImplementedException();
        }
    }
}