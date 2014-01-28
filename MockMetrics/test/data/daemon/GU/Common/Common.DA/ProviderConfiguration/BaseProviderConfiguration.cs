using System;
using BLToolkit.Data.DataProvider;

namespace Common.DA.ProviderConfiguration
{
    /// <summary>
    /// Класс, предназначенный для хранения конфигурации подключения к БД.
    /// </summary>
    [Serializable]
    public abstract class BaseProviderConfiguration : IProviderConfiguration
    {
        /// <summary>
        /// Класс, предназначенный для хранения конфигурации подключения к БД.
        /// </summary>
        /// <param name="configurationName">Имя конфигурации</param>
        /// <param name="connectionStringTemplate">Шаблон строки подключения</param>
        protected BaseProviderConfiguration(string configurationName, 
                                            string connectionStringTemplate,
                                            string server,
                                            int port)
        {
            ConfigurationName = configurationName;
            ConnectionStringTemplate = connectionStringTemplate;
            Server = server;
            Port = port;
        }

        /// <summary>
        /// Шаблон строки подключения.
        /// </summary>
        public string ConnectionStringTemplate { get; protected set; }

        /// <summary>
        /// Возвращает имя конфигурации подключения.
        /// </summary>
        public string ConfigurationName { get; protected set; }

        /// <summary>
        /// Возвращает провайдер подключения к БД.
        /// </summary>
        public abstract DataProviderBase DataProvider { get; }
        
        /// <summary>
        /// Адрес сервера
        /// </summary>
        public string Server { get; protected set; }

        /// <summary>
        /// Порт подключения.
        /// </summary>
        public int Port { get; protected set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Возврашает строку подключения к БД.
        /// </summary>
        /// <returns>Строка подключения</returns>
        public abstract string GetConnectionString();

        #region ICloneable Members

        /// <summary>
        /// Возвращает клон объекта.
        /// </summary>
        /// <returns>Клон объекта</returns>
        public abstract object Clone();

        #endregion
    }
}
