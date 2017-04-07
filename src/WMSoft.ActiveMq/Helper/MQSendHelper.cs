using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq.Helper
{
    public class MQSendHelper
    {
        #region Log相关
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="subject"></param>
        public static void TestLog(string msg)
        {
            var message = new Models.LogMessage()
            {
                MessageType = MessageType.TestLog,
                Message = "",
            };
            Provider.Instance.LogProducer.Send(message);
        }
        #endregion
        
    }
}
