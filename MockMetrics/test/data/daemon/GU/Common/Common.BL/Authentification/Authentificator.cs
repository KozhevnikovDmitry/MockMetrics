using System;
using System.Net.NetworkInformation;
using System.Text;

using Common.DA.Interface;
using Common.DA.ProviderConfiguration;

namespace Common.BL.Authentification
{
    /// <summary>
    /// Класс, ответсвенный за аутентификацию пользователя в приложении.
    /// </summary>
    public class Authentificator : IAuthentificator
    {
        /// <summary>
        /// Конфигурация подключения к базе данных.
        /// </summary>
        protected readonly IProviderConfiguration _configuration;

        /// <summary>
        /// Объект, проверяющий подключение к БД.
        /// </summary>
        protected readonly IConfigurationTester DataAccessLayerInitializer;

        /// <summary>
        /// Класс, ответсвенный за аутентификацию пользователя в приложении.
        /// </summary>
        /// <param name="configuration">Конфигурация подключения к базе данных</param>
        /// <param name="dataAccessLayerInitializer">Объект, проверяющий подключение к БД.</param>
        public Authentificator(IProviderConfiguration configuration, IConfigurationTester dataAccessLayerInitializer)
        {
            _configuration = configuration;
            this.DataAccessLayerInitializer = dataAccessLayerInitializer;
        }

        /// <summary>
        /// Сообщение об ошибке аутентификации
        /// </summary>
        public string ErrorMessage { get; protected set; }

        #region Implementation of IAuthentificator

        /// <summary>
        /// Проводит аутентификацию пользователя, возвращает флаг успешности аутентификации.
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Флаг успешности аутентификации</returns>
        public virtual bool AuthentificateUser(string username, string password)
        {
            _configuration.User = (username ?? string.Empty).ToLower();
            _configuration.Password = password ?? string.Empty;
            var result = this.DataAccessLayerInitializer.FullTestConfiguration(_configuration);
            if (!result.IsSuccessful)
            {
                ErrorMessage = result.ErrorMessage;
                //throw result.Exception;
            }

            return result.IsSuccessful;
        }

        #endregion
    }
}
