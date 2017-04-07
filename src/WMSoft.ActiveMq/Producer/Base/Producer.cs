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
    public class Producer
    {
        ServiceConfig _config = null;
        internal Producer(string name)
        {
            _config = SectionController.Default.GetConfig(name);
        }
        public ServiceConfig ServiceConfig
        {
            get { return _config; }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="arr"></param>
        public virtual void Send(byte[] arr)
        {
            if (_config == null || arr == null || arr.Length < 1)
                return;
            //IConnectionFactory factory = new ConnectionFactory(_config.ActiveMQUri);
            var connection = ConnectionPool.GetConnection(_config.Name, _config.ActiveMQUri);
            using (ISession session = connection.CreateSession())
            {
                IMessageProducer prod = session.CreateProducer(
                    new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(_config.TopicOrQueueName));

                IBytesMessage msg = prod.CreateBytesMessage();

                msg.Content = arr;
                prod.Send(msg, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
            }
        }
    }
}
