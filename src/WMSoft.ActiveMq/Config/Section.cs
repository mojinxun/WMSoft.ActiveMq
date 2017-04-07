using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq.Config
{
    public class Section : ConfigurationSection
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        [ConfigurationProperty("services", IsRequired = true)]
        public ServiceCollection Services
        {
            get { return this["services"] as ServiceCollection; }
        }
    }
    /// <summary>
    /// thrift service collection
    /// </summary>
    [ConfigurationCollection(typeof(ServiceConfig), AddItemName = "service")]
    public class ServiceCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// create new element
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfig();
        }
        /// <summary>
        /// 获取指定元素的Key。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ServiceConfig).Name;
        }
        /// <summary>
        /// 获取指定位置的对象。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ServiceConfig this[int i]
        {
            get { return BaseGet(i) as ServiceConfig; }
        }
        /// <summary>
        /// 获取指定key的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ServiceConfig Get(string key)
        {
            return BaseGet(key) as ServiceConfig;
        }
    }
    /// <summary>
    /// service config
    /// </summary>
    public class ServiceConfig : ConfigurationElement
    {
        /// <summary>
        /// name
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (String)this["name"]; }
        }
        /// <summary>
        /// activeMQUri
        /// </summary>
        [ConfigurationProperty("activeMQUri", IsRequired = true)]
        public string ActiveMQUri
        {
            get { return (String)this["activeMQUri"]; }
        }
        /// <summary>
        /// topicOrQueueName
        /// </summary>
        [ConfigurationProperty("topicOrQueueName", IsRequired = true)]
        public string TopicOrQueueName
        {
            get { return (String)this["topicOrQueueName"]; }
        }
        /// <summary>
        /// desc
        /// </summary>
        [ConfigurationProperty("desc", IsRequired = false)]
        public string Desc
        {
            get { return (String)this["desc"]; }
        }
    }
}