using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WMSoft.ActiveMq
{
    public class ConnectionPool
    {
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="brokerUri"></param>
        /// <returns></returns>
        public static IConnection GetConnection(string name, string brokerUri)
        {
            lock (connections)
            {
                var kv = connections.FirstOrDefault(oo => oo.Key == name);
                if (kv.Value == null)
                    return Connection(name, brokerUri);
                return kv.Value.Item2;
            }
        }

        /// <summary>
        /// 清除连接
        /// </summary>
        /// <param name="configEnum"></param>
        public static void ClearConnection(string name)
        {
            lock (connections)
            {
                connections.Remove(name);
            }
        }

        /// <summary>
        /// 清除所有连接
        /// </summary>
        public static void ClearConnections()
        {
            lock (connections)
            {
                connections = new Dictionary<string, Tuple<string, IConnection>>();
            }
        }


        #region 初始化
        /// <summary>
        /// name brokeruri connection
        /// </summary>
        private static Dictionary<string, Tuple<string, IConnection>> connections = new Dictionary<string, Tuple<string, IConnection>>();
        static ConnectionPool() { }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="brokerUri"></param>
        /// <returns></returns>
        private static IConnection Connection(string name, string brokerUri)
        {
            if (string.IsNullOrEmpty(brokerUri))
            {
                throw new ArgumentException("brokeruri null");
            }

            lock (connections)
            {
                var factory = new ConnectionFactory(brokerUri);
                var connection = factory.CreateConnection();
                connection.ConnectionInterruptedListener += Connection_ConnectionInterruptedListener;
                connection.ExceptionListener += Connection_ExceptionListener;
                connections.Add(name, Tuple.Create(brokerUri, connection));
                return connection;
            }
        }

        /// <summary>
        /// 重连
        /// </summary>
        private static void Reconnection()
        {
            lock (connections)
            {
                foreach (var item in connections)
                {
                    var brokerUri = item.Value.Item1;
                    if (string.IsNullOrEmpty(brokerUri))
                        continue;

                    var factory = new ConnectionFactory(brokerUri);
                    var connection = factory.CreateConnection();

                    connection.ConnectionInterruptedListener += Connection_ConnectionInterruptedListener;
                    connection.ExceptionListener += Connection_ExceptionListener;
                    connections[item.Key] = Tuple.Create(brokerUri, connection);
                }
            }
        }

        /// <summary>
        /// 异常触发
        /// </summary>
        /// <param name="exception"></param>
        private static void Connection_ExceptionListener(Exception exception)
        {
            Reconnection();
        }

        /// <summary>
        /// 连接中断触发
        /// </summary>
        private static void Connection_ConnectionInterruptedListener()
        {
            Reconnection();
        }

        #endregion
    }
}
