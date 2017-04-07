using Apache.NMS;
using WMSoft.ActiveMq.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq
{
    /// <summary>
    /// 消息队列生产者
    /// </summary>
    public class Consumer
    {
        ServiceConfig _config = null;
        internal Consumer(string name)
        {
            _config = SectionController.Default.GetConfig(name);
        }

        public virtual void Start(Action<IMessage> callback = null)
        {
            try
            {
                var connection = ConnectionPool.GetConnection(_config.Name, _config.ActiveMQUri);
                connection.Start();

                var session = connection.CreateSession();
                var consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(_config.TopicOrQueueName), null);
                if (callback != null)
                    consumer.Listener += new MessageListener(callback);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
