using UnityEngine;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


using System;
using System.Collections;

namespace MKdev.MqttForUnity
{
    public class MqttConnector : MonoBehaviour
    {
        public string BrokerIp = "192.168.0.2";
        public string username = "";
        public string password = "";


        private MqttClient client;
        private Dictionary<string, List<IMqttTopicReceiver>> DictTopicReceiver;

        private Queue<MqttMsgPublishEventArgs> eventQueue;

        void Start()
        {
            DictTopicReceiver = new Dictionary<string, List<IMqttTopicReceiver>>();

            eventQueue = new Queue<MqttMsgPublishEventArgs>();
            StartCoroutine("workQueueAndCall");
            
            // create client instance 
            //client = new MqttClient(IPAddress.Parse("143.185.118.233"),8080 , false , null ); 
            client = new MqttClient(BrokerIp);

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId, username, password);
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
                listReceiver = new List<IMqttTopicReceiver>();
                listReceiver.Add(receiver);
                this.DictTopicReceiver.Add(topic, listReceiver);

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

