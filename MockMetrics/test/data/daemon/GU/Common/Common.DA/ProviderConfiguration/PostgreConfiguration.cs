using BLToolkit.Data.DataProvider;

namespace Common.DA.ProviderConfiguration
{
    /// <summary>
    /// Класс, предназначенный для хранения конфигурации подключения к БД PostgreSQL.
    /// </summary>
    public class PostgreConfiguration : BaseProviderConfiguration
    {
        public PostgreConfiguration(string configurationName,
                                    string connectionStringTemplate,
                                    string server,
                                    int port,
                                    string database)
            :base(configurationName, connectionStringTemplate, server, port)
        {
            Database = database;
        }

        public override DataProviderBase DataProvider
        {
            get
            {
                return new PostgreSQLDataProvider();
            }
        }

        public override string GetConnectionString()
        {
            return string.Format(ConnectionStringTemplate, Server, Port, Database, User, Password);
        }

        public string Database { get; protected set; }

        #region ICloneable Members

        public override object Clone()
        {
            return new PostgreConfiguration(this.ConfigurationName,
                                            this.ConnectionStringTemplate,
                                            this.Server,
                                            this.Port,
                                            this.Database);
        }

        #endregion
    }
}
