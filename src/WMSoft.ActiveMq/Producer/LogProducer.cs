using WMSoft.ActiveMq.Helper;
using WMSoft.ActiveMq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq
{
    public sealed class LogProducer : Producer
    {
        internal LogProducer()
            : base(ConfigEnum.testlog)
        {
        }
        /// <summary>
        /// 发送订单消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(LogMessage message)
        {
            if (message == null)
                return;
            Send(ByteHelper.ObjectToBytes(message));
        }
    }
}
