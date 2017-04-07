using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq
{
    /// <summary>
    /// 消费者
    /// </summary>
    public class ProducerHelper
    {
        /// <summary>
        /// Producer
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        public static void Send(IConnection connection, string topic, byte[] content)
        {
            if (connection == null)
                throw new ArgumentNullException("connection null");
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentNullException("topic null");
            if (content == null)
                throw new ArgumentNullException("content null");

            using (ISession session = connection.CreateSession())
            {
                var prod = session.CreateProducer(
                    new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(topic));

                var msg = prod.CreateBytesMessage();

                msg.Content = content;
                prod.Send(msg, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
            }
        }
    }
}
