using System;
using BLToolkit.Data.DataProvider;

namespace Common.DA.ProviderConfiguration
{
    /// <summary>
    /// Интерфейс классов, хранящих конфигурацию провайдера подключения к базе данных.
    /// </summary>
    public interface IProviderConfiguration : ICloneable
    {
        /// <summary>
        /// Возвращает или устанавливает шаблон строки подключения к базе данных.
        /// </summary>
        string ConnectionStringTemplate { get; }
        
        /// <summary>
        /// Возвращает имя конфигурации подключения.
        /// </summary>
        string ConfigurationName { get; }

        /// <summary>
        /// Возвращает провайдер подключения к БД.
        /// </summary>
        DataProviderBase DataProvider { get; }
        
        /// <summary>
        /// Адрес сервера
        /// </summary>
        string Server { get; }
        
        /// <summary>
        /// Номер порта.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Возвращает строку подключения к базе данных.
        /// </summary>
        /// <returns>Cтроку подключения к базе данных</returns>
        string GetConnectionString();
    }
}
