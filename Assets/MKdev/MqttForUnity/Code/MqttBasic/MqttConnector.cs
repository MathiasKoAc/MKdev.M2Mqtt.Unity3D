using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine;

using MKdev.ServerConfig;


namespace MKdev.MqttForUnity
{
    /**
     * The MqttConnector connects the UnityScene with the MqttBroker
     * The Mqtt Connector uses Queues to seperate the thread-shedule from the coroutine-world
     */ 
    public class MqttConnector : MonoBehaviour
    {
        public AbsServerConfig ServerConfig;

        private MqttClient client;
        private Dictionary<string, List<IMqttTopicReceiver>> DictTopicReceiver;

        private Queue<MqttMsgPublishEventArgs> receiverQueue;
        private Queue<MqttMsgPublishEventArgs> senderQueue;

        void Start()
        {
            DictTopicReceiver = new Dictionary<string, List<IMqttTopicReceiver>>();

            InitAndStartInnerCoroutines();

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
                    this.StopInnerCoroutines();
                    Debug.LogError("MqttConnector can't Connect to the Broker. Check ServerConfig IP, Username, Password.");
                }
            }
            catch (Exception se)
            {
                this.StopInnerCoroutines();
                Debug.LogError(new StringBuilder("MqttConnector can't Connect to the Broker on IP: ").Append(ServerConfig.GetServerIp()));
                Debug.LogException(se);
            }
            
        }

        /**
         * The Methode client_MqttMsgPublishReceived is called, if a MqttMessage arrieved and put it into the receiverQueue
         * client_MqttMsgPublishReceived seperats the thread which puts the message in the queue from working the message
         */
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.Log(new StringBuilder("Received: ").Append(System.Text.Encoding.UTF8.GetString(e.Message)).Append(" Topic: ").Append(e.Topic));
            receiverQueue.Enqueue(e);
        }

        private void InitAndStartInnerCoroutines()
        {
            //start of the concurrency (Nebenläufig) for receiving Messages
            receiverQueue = new Queue<MqttMsgPublishEventArgs>();
            //start of the concurrency (Nebenläufig) for sending Messages
            senderQueue = new Queue<MqttMsgPublishEventArgs>();
            StartInnerCoroutines();
        }

        private void StartInnerCoroutines()
        {
            //start of the concurrency (Nebenläufig) for receiving Messages
            StartCoroutine("workReceiverQueueAndCall");
            //start of the concurrency (Nebenläufig) for sending Messages
            StartCoroutine("workSenderQueueAndSend");
        }

        private void StopInnerCoroutines()
        {
            StopCoroutine("workReceiverQueueAndCall");
            StopCoroutine("workSenderQueueAndSend");
        }

        /**
         * workReveiverQueueAndCall is a Inner Coroutine, which loops over the ReceivingQueue to call the Subscriper in this App
         * workSenderQueueAndSend seperats the thread which puts the message in the queue from working the message
         */
        private IEnumerator workReceiverQueueAndCall()
        {
            while(true)
            {
                if (receiverQueue.Count > 0)
                {
                    MqttMsgPublishEventArgs e = receiverQueue.Dequeue();
                   
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

        /**
         * workSenderQueueAndSend is a Inner Coroutine, which loops over the senderQueue to send a Message if there is one
         * workSenderQueueAndSend seperats the call to send from the sending its self
         */
        private IEnumerator workSenderQueueAndSend()
        {
            while(true)
            {
                if(senderQueue.Count > 0)
                {
                    MqttMsgPublishEventArgs mqttMessage = senderQueue.Dequeue();
                    client.Publish(mqttMessage.Topic, mqttMessage.Message, mqttMessage.QosLevel, mqttMessage.Retain);
                    Debug.Log(new StringBuilder("SEND# Topic: ").Append(mqttMessage.Topic));
                }
                yield return null;
            }
        }

        public void PublishMessage(string topic, string Message, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE, bool retain = false)
        {
            Debug.Log("sending init");
            this.senderQueue.Enqueue(new MqttMsgPublishEventArgs(topic, System.Text.Encoding.UTF8.GetBytes(Message), false, (byte) MqttQOS_Level, retain));
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

