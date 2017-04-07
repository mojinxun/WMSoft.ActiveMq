using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq
{
    public class Provider
    {
        #region private field
        private static Provider _instance = null;
        private static object _lock = new object();
        private LogProducer logProducer = null;
        #endregion private field
        Provider()
        {

        }

        /// <summary>
        /// 实例
        /// </summary>
        public static Provider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Provider();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// OrderProducer
        /// </summary>
        public LogProducer LogProducer
        {
            get
            {
                if (logProducer != null)
                    return logProducer;
                logProducer = new LogProducer();
                return logProducer;
            }
        }
    }
}