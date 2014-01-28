using System;
using BLToolkit.Data.DataProvider;

namespace Common.DA.ProviderConfiguration
{
    /// <summary>
    /// Класс, предназначенный для хранения конфигурации  подключения к БД Oracle.
    /// </summary>
    [Serializable]
    public class OracleConfiguration : BaseProviderConfiguration
    {
        
        public OracleConfiguration(string configurationName,
                                   string connectionStringTemplate,
                                   string server,
                                   int port,
                                   string sid)
            :base(configurationName, connectionStringTemplate, server, port)
        {
            Sid = sid;
        }

        public override DataProviderBase DataProvider
        {
            get
            {
                return null;// new DevartDataProvider();
            }
        }

        /// <summary>
        /// SID базы данных.
        /// </summary>
        public string Sid { get; protected set; }

        /// <summary>
        ///  Возврашает строку подключения к БД.
        /// </summary>
        /// <returns>Строка подключения</returns>
        public override string GetConnectionString()
        {
            return string.Format(ConnectionStringTemplate, Server, Port, Sid, User, Password);
        }

        #region ICloneable Members

        public override object Clone()
        {
            return new PostgreConfiguration(ConfigurationName,
                                            ConnectionStringTemplate,
                                            Server,
                                            Port,
                                            Sid);
        }

        #endregion
    }
}
