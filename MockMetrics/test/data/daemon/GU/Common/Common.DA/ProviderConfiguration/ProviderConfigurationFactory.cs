using System;
using System.Collections.Generic;
using System.Configuration;
using Common.Types.Exceptions;

namespace Common.DA.ProviderConfiguration
{
    /// <summary>
    /// Фабрика конфигураций провайдера BLT для подключения к базе данных.
    /// </summary>
    public class ProviderConfigurationFactory
    {
        /// <summary>
        /// Словарь конфигураций.
        /// </summary>
        private readonly Dictionary<string, IProviderConfiguration> _configurations = new Dictionary<string, IProviderConfiguration>();

        /// <summary>
        /// Фабрика конфигураций провайдера BLT для подключения к базе данных.
        /// </summary>
        /// <exception cref="GUException">Не удалось прочитать конфигурационный файл.</exception>
        public ProviderConfigurationFactory()
        {
            try
            {
                _configurations["Oracle"] = new OracleConfiguration("Oracle",
                                                                     ConfigurationManager.AppSettings["OracleConnectionStringTemplate"],
                                                                     ConfigurationManager.AppSettings["OracleServer"],
                                                                     Convert.ToInt32(ConfigurationManager.AppSettings["OraclePort"]),
                                                                     ConfigurationManager.AppSettings["OracleSid"]);

                _configurations["Postgre"] = new PostgreConfiguration("Postgre",
                                                                      ConfigurationManager.AppSettings["PostgreConnectionStringTemplate"],
                                                                      ConfigurationManager.AppSettings["PostgreServer"],
                                                                      Convert.ToInt32(ConfigurationManager.AppSettings["PostgrePort"]),
                                                                      ConfigurationManager.AppSettings["PostgreDatabase"]);
            }
            catch (Exception ex)
            {
                throw new GUException("Не удалось прочитать конфигурационный файл.", ex);
            }
        }

        /// <summary>
        /// Возвращает конфигурацию провайдера BLT по имени конфигурации.
        /// </summary>
        /// <param name="name">Имя конфигурации</param>
        /// <returns>Конфигурация провайдера подключения</returns>
        /// <exception cref="DALException">Нет зарегистрированной конфигурации подключения к БД по названием</exception>
        public IProviderConfiguration GetConfiguration(string name)
        {
            if (!_configurations.ContainsKey(name))
            {
                throw new DALException("Нет зарегистрированной конфигурации подключения к БД по названием" + name);
            }
            return _configurations[name];
        }
    }
}
