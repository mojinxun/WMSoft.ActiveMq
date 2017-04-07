using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WMSoft.ActiveMq.Config
{
    internal class SectionController
    {
        public static SectionController Default = new SectionController();
        private static Section configSection = null;

        static SectionController()
        {
            string configPath = @"dllConfigs\WMSoft.ActiveMq.dll.config";
            var config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
            {
                ExeConfigFilename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath)
            }, ConfigurationUserLevel.None);
            if (config == null)
                throw new ArgumentNullException("WMSoft.ActiveMq config");

            configSection = config.GetSection("activeMQConfig") as Section;
        }

        public ServiceConfig GetConfig(string name)
        {
            return configSection.Services.Get(name);
        }

        public List<ServiceConfig> GetConfigs()
        {
            var serviceConfigs = new List<ServiceConfig>();
            foreach (var item in configSection.Services)
            {
                serviceConfigs.Add(item as ServiceConfig);
            }
            return serviceConfigs;
        }
    }
}
