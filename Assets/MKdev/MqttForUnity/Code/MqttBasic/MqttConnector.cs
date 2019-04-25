using UnityEngine;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.Collections;
using MKdev.ServerConfig;


namespace MKdev.MqttForUnity
{
    /**
     * It connects the UnityScene with the Broker
     */ 
    public class MqttConnector : MonoBehaviour
    {
        public AbsServerConfig ServerConfig;

        private MqttClient client;
        private Dictionary<string, List<IMqttTopicReceiver>> DictTopicReceiver;

        private Queue<MqttMsgPublishEventArgs> eventQueue;

        void Start()
        {
            DictTopicReceiver = new Dictionary<string, List<IMqttTopicReceiver>>();

            eventQueue = new Queue<MqttMsgPublishEventArgs>();
            StartCoroutine("workQueueAndCall");

            try
            {
                // create client instance 
                //client = new MqttClient(IPAddress.Parse("143.185.118.233"),8080 , false , null ); 
                client = new MqttClient(this.ServerConfig.GetServerIp());

                // register to message received 
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

                string clientId = Guid.NewGuid().ToString();
                byte conacct = client.Connect(clientId, ServerConfig.GetUsername(), ServerConfig.GetPassword());

                if (conacct != MqttMsgConnack.CONN_ACCEPTED)
                {
                    StopCoroutine("workQueueAndCall");
                    Debug.LogError("MqttConnector can't Connect to the Broker. Check ServerConfig IP, Username, Password.");
                }
            }
            catch (Exception se)
            {
                StopCoroutine("workQueueAndCall");
                Debug.LogError("MqttConnector can't Connect to the Broker on IP: " + ServerConfig.GetServerIp());
                Debug.LogException(se);
            }
            
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message) + " Topic: "+ e.Topic);
            eventQueue.Enqueue(e);
            
        }

        private IEnumerator workQueueAndCall()
        {
            while(true)
            {
                if (eventQueue.Count > 0)
                {
                    MqttMsgPublishEventArgs e = eventQueue.Dequeue();
                   
                    List<IMqttTopicReceiver> listReceiver = new List<IMqttTopicReceiver>();
                    if (DictTopicReceiver.TryGetValue(e.Topic.ToLower(), out listReceiver))
                    {
                        if (listReceiver != null)
                        {
                            foreach (IMqttTopicReceiver receiver in listReceiver)
                            {
                                receiver.OnReceivedMessage(e.Topic, e.Message);
                            }
                        }
                    }
                }
                yield return null;
            }
        }

        public void PublishMessage(string topic, string Message, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE)
        {
            Debug.Log("sending...");
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(Message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("sent");
        }

        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE)
        {
            AddTopicReceiver(topic, receiver, (byte)MqttQOS_Level);
        }

        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, byte MqttQOS_Level)
        {
            topic = topic.ToLower();

            //this style for 3.5.Net
            List<IMqttTopicReceiver> listReceiver = new List<IMqttTopicReceiver>();
            if(this.DictTopicReceiver.TryGetValue(topic, out listReceiver))
            {
                listReceiver.Add(receiver);
                client.Subscribe(new string[] { topic }, new byte[] { MqttQOS_Level });

            }
            else
            {
                listReceiver = new List<IMqttTopicReceiver>();
                listReceiver.Add(receiver);
                this.DictTopicReceiver.Add(topic, listReceiver);

                client.Subscribe(new string[] { topic }, new byte[] { MqttQOS_Level });

            }
        }
    }
}

