using Apache.NMS;
using WMSoft.ActiveMq.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMSoft.ActiveMq.Enums;
using Apache.NMS.ActiveMQ.Commands;

namespace WMSoft.ActiveMq
{
    /// <summary>
    /// 消息队列生产者
    /// </summary>
    public class Producer
    {
        #region Private Members
        IConnection connection = null;

        ServiceConfig config = null;
        ActiveMQDestination destination = null;
        #endregion

        #region Constructor
        public Producer(string name, EnumMqType mqType = EnumMqType.queue)
        {
            #region Name
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name can not null");

            config = SectionController.Default.GetConfig(name);
            if (config == null || string.IsNullOrEmpty(config.Name))
                throw new ArgumentOutOfRangeException($"name[{name}] not find in WMSoft.ActiveMq.dll.configs");

            if (string.IsNullOrEmpty(config.TopicOrQueueName))
                throw new ArgumentNullException("topicOrQueueName can not null");
            #endregion

            #region MqType
            switch (mqType)
            {
                case EnumMqType.queue:
                    destination = new ActiveMQQueue(config.TopicOrQueueName);
                    break;
                case EnumMqType.topic:
                    destination = new ActiveMQTopic(config.TopicOrQueueName);
                    break;
                default:
                    throw new ArgumentNullException("mqType not exist");
            }
            #endregion

            #region GetConnection
            connection = ConnectionPool.GetConnection(config.Name, config.ActiveMQUri);
            #endregion
        }

        public Producer(string brokerUri, string topicOrQueueName, EnumMqType mqType = EnumMqType.queue)
        {
            #region brokerUri
            if (string.IsNullOrEmpty(brokerUri))
                throw new ArgumentNullException("brokerUri can not null");
            #endregion

            #region topicOrQueueName
            if (string.IsNullOrEmpty(topicOrQueueName))
                throw new ArgumentNullException("topicOrQueueName can not null");
            #endregion

            #region MqType
            switch (mqType)
            {
                case EnumMqType.queue:
                    destination = new ActiveMQQueue(config.TopicOrQueueName);
                    break;
                case EnumMqType.topic:
                    destination = new ActiveMQTopic(config.TopicOrQueueName);
                    break;
                default:
                    throw new ArgumentNullException("mqType not exist");
            }
            #endregion

            #region GetConnection
            connection = ConnectionPool.GetConnection(topicOrQueueName, brokerUri);
            #endregion
        }
        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="arr"></param>
        public virtual void Send(byte[] arr)
        {
            if (config == null || arr == null || arr.Length < 1)
                return;
            
            using (var session = connection.CreateSession())
            {
                var prod = session.CreateProducer(destination);
                var msg = prod.CreateBytesMessage();

                msg.Content = arr;
                prod.Send(msg, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
            }
        }
    }
}
