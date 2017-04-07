using Apache.NMS;
using WMSoft.ActiveMq.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS.ActiveMQ.Commands;

namespace WMSoft.ActiveMq
{
    /// <summary>
    /// 消息队列生产者
    /// </summary>
    public class Consumer
    {
        #region Private Method
        IConnection connection = null;
        ISession session = null;
        IMessageConsumer consumer = null;
        Action<IMessage> callback = null;

        ServiceConfig config = null;
        #endregion

        #region Construct
        public Consumer(string name, Action<IMessage> action = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name need");

            config = SectionController.Default.GetConfig(name);
            callback = action;
        }
        #endregion

        /// <summary>
        /// Start
        /// </summary>
        public virtual void Start()
        {
            try
            {
                connection = ConnectionPool.GetConnection(config.Name, config.ActiveMQUri);
                connection.Start();

                session = connection.CreateSession();
                consumer = session.CreateConsumer(new ActiveMQQueue(config.TopicOrQueueName), null);
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
