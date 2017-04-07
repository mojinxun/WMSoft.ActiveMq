using WMSoft.ActiveMq.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq
{
    public sealed class LogProducer<T> : Producer where T : class
    {
        internal LogProducer()
            : base("")
        {
        }
        /// <summary>
        /// 发送订单消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(T message)
        {
            if (message == null)
                return;
            Send(ByteHelper.ObjectToBytes(message));
        }
    }
}
