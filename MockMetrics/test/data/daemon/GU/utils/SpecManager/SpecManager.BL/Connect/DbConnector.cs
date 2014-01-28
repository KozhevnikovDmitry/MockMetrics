using System.Collections.Generic;
using System.Configuration;

using SpecManager.BL.Exceptions;

namespace SpecManager.BL.Connect
{
    public class DbConnector
    {
        public DbConnector()
        {
            ConfigNameList = new List<string>();
            foreach (string settingKey in ConfigurationManager.AppSettings.Keys)
            {
                if (settingKey.EndsWith("ConnectionConfig"))
                {
                    ConfigNameList.Add(ConfigurationManager.AppSettings[settingKey]);
                }
            }
        }

        public List<string> ConfigNameList { get; private set; }

        public string GetConnectionsString(string configName, string user, string password)
        {
            if (!ConfigNameList.Contains(configName))
            {
                throw new BLLException(string.Format("Неизвестная конфигурация {0}", configName));
            }
            
            return string.Format(ConfigurationManager.AppSettings["PostgreConnectionStringTemplate"],
                                 ConfigurationManager.AppSettings[configName + "PostgreServer"],
                                 ConfigurationManager.AppSettings[configName + "PostgrePort"],
                                 ConfigurationManager.AppSettings[configName + "PostgreDatabase"],
                                 user,
                                 password);
        }

        public string GetConnectionsString(string server, string port, string database, string user, string password)
        {
            return string.Format(ConfigurationManager.AppSettings["PostgreConnectionStringTemplate"],
                                 server,
                                 port,
                                 database,
                                 user,
                                 password);
        }

        public ConnectConfig GetConnectConfig(string configName)
        {
            if (!ConfigNameList.Contains(configName))
            {
                throw new BLLException(string.Format("Неизвестная конфигурация {0}", configName));
            }

            return new ConnectConfig
                       {
                           Server = ConfigurationManager.AppSettings[configName + "PostgreServer"],
                           Port = ConfigurationManager.AppSettings[configName + "PostgrePort"],
                           Database = ConfigurationManager.AppSettings[configName + "PostgreDatabase"],
                           User = ConfigurationManager.AppSettings[configName + "PostgreUser"],
                           Password = ConfigurationManager.AppSettings[configName + "PostgrePassword"]
                       };
        }
    }

    public class ConnectConfig  
    {
        public string Server { get; set; }

        public string Port { get; set; }

        public string Database { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
