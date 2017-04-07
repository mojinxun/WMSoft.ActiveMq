using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using System;
using WMSoft.ActiveMq.Config;
using WMSoft.ActiveMq.Enums;

namespace WMSoft.ActiveMq
{
    /// <summary>
    /// 消息队列生产者
    /// </summary>
    public class Consumer
    {
        #region Private Members
        IConnection connection = null;
        ISession session = null;
        IMessageConsumer consumer = null;
        ActiveMQDestination destination = null;
        Action<IMessage> callback = null;

        ServiceConfig config = null;

        #endregion

        #region Constructor
        public Consumer(string name, EnumMqType mqType = EnumMqType.queue, Action<IMessage> action = null)
        {
            #region Name
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name need");

            config = SectionController.Default.GetConfig(name);
            if (config == null || string.IsNullOrEmpty(config.Name))
                throw new ArgumentOutOfRangeException($"name[{name}] not find in WMSoft.ActiveMq.dll.configs");

            if (string.IsNullOrEmpty(config.TopicOrQueueName))
                throw new ArgumentNullException("topicOrQueueName is not exist");
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

            #region Action
            callback = action;
            #endregion

            #region GetConnection
            connection = ConnectionPool.GetConnection(config.Name, config.ActiveMQUri);
            #endregion
        }


        public Consumer(string brokerUri, string topicOrQueueName, EnumMqType mqType = EnumMqType.queue, Action<IMessage> action = null)
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
                    destination = new ActiveMQQueue(topicOrQueueName);
                    break;
                case EnumMqType.topic:
                    destination = new ActiveMQTopic(topicOrQueueName);
                    break;
                default:
                    throw new ArgumentNullException("mqType not exist");
            }
            #endregion

            #region Action
            callback = action;
            #endregion

            #region GetConnection
            connection = ConnectionPool.GetConnection(topicOrQueueName, brokerUri);
            #endregion
        }
        #endregion

        /// <summary>
        /// Start
        /// </summary>
        public virtual void Start()
        {
            try
            {
                if (!connection.IsStarted)
                    connection.Start();

                session = connection.CreateSession();
                consumer = session.CreateConsumer(destination, null);
                if (callback != null)
                    consumer.Listener += new MessageListener(callback);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Stop
        /// </summary>
        public virtual void Stop()
        {
            if (consumer != null)
            {
                consumer.Listener -= new MessageListener(callback);
                consumer.Dispose();
                consumer = null;
            }
            if (session != null)
            {
                session.Close();
                session.Dispose();
            }
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
