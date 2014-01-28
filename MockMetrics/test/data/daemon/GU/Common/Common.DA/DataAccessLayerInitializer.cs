using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;

using BLToolkit.Data;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;

using Npgsql;

namespace Common.DA
{
    /// <summary>
    /// Класс, предназначенный для проведения инициализации базы данных
    /// </summary>
    public class DataAccessLayerInitializer : IDataAccessLayerInitializer
    {
        /// <summary>
        /// Проверяет возможность поключения в БД по заданной конфигурации 
        /// </summary>
        /// <param name="config">Конфигурация подключения</param>
        /// <returns>Флаг возможности подключения</returns>
        public bool TestConfiguration(IProviderConfiguration config)
        {
            try
            {
                DbManager.AddDataProvider(config.ConfigurationName, config.DataProvider);
                DbManager.AddConnectionString(config.ConfigurationName, config.GetConnectionString());
                using (var db = new DbManager(config.ConfigurationName))
                {
                    return db.Connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет возможность поключения в БД по заданной конфигурации 
        /// </summary>
        /// <param name="config">Конфигурация подключения</param>
        /// <returns>Информация о проверке подключения</returns>
        public ConfigurationTestResult FullTestConfiguration(IProviderConfiguration config)
        {
            try
            {
                DbManager.AddDataProvider(config.ConfigurationName, config.DataProvider);
                DbManager.AddConnectionString(config.ConfigurationName, config.GetConnectionString());
                using (var db = new DbManager(config.ConfigurationName))
                {
                    var connection = db.Connection;
                    return new ConfigurationTestResult { IsSuccessful = true };
                    
                }
            }
            catch (DataException ex)
            {
                return ProvideFailTest(ex);
            }
            catch (Exception ex)
            {
                return new ConfigurationTestResult
                               {
                                   IsSuccessful = false,
                                   ErrorMessage = ex.Message,
                                   Exception = ex
                               };
            }
        }

        /// <summary>
        /// Возвращает менеджер базы данных, до которой удалось достучаться.
        /// </summary>
        /// <returns>Менеджер БД</returns>
        public IDomainDbManager GetDbManager(Assembly blAssembly)
        {
            return (IDomainDbManager)Activator.CreateInstance(blAssembly.GetTypes().Single(t => typeof(IDomainDbManager).IsAssignableFrom(t)));
        }

        /// <summary>
        /// Инициализирует слой доступа к данным.
        /// </summary>
        /// <param name="config">Конфигурация подключения</param>
        /// <param name="dataModelAssembly">Сборка с дата классами</param>
        /// <param name="domainObjectInitilizer">Инициализатор доменных объектов</param>
        public void Initialize(IProviderConfiguration config,
                               Assembly dataModelAssembly,
                               IDomainObjectInitializer domainObjectInitilizer)
        {
            DbManager.TurnTraceSwitchOn();
            DbManager.WriteTraceLine = (message, displayName) => Debug.WriteLine(message, displayName); // any logging function
            DbManager.AddDataProvider(config.ConfigurationName, config.DataProvider);
            DbManager.AddConnectionString(config.ConfigurationName, config.GetConnectionString());
            BLToolkit.Common.Configuration.NullableValues.String = null;
            DomainObjectInitializerContainer.Instance.RegisterInitializer(dataModelAssembly, domainObjectInitilizer);
        }

        public void Initialize(IProviderConfiguration config,
                               Dictionary<Assembly,IDomainObjectInitializer> initDictionary)
        {
            DbManager.TurnTraceSwitchOn();
            DbManager.WriteTraceLine = (message, displayName) => Debug.WriteLine(message, displayName); // any logging function
            DbManager.AddDataProvider(config.ConfigurationName, config.DataProvider);
            DbManager.AddConnectionString(config.ConfigurationName, config.GetConnectionString());
            BLToolkit.Common.Configuration.NullableValues.String = null;
            foreach (var initItem in initDictionary)
            {
                DomainObjectInitializerContainer.Instance.RegisterInitializer(initItem.Key, initItem.Value);
            }
        }

        /// <summary>
        /// Формирует и возвращает информацию о провале подключения к базе данных
        /// </summary>
        /// <param name="dataException">Исключение с ошибкой подключения</param>
        /// <returns>Информация о провале подключения</returns>
        private ConfigurationTestResult ProvideFailTest(DataException dataException)
        {
            var result = new ConfigurationTestResult
            {
                IsSuccessful = false,
                Exception = dataException
            };

            var errorCode = (dataException.InnerException as NpgsqlException).Code;

            if (string.IsNullOrEmpty(errorCode))
            {
                result.ErrorMessage = "Сервер базы данных недоступен";
                return result;
            }
            
            result.ErrorMessage = errorCode == "28000" ? "Неверный логин или пароль" : dataException.Message;
            return result;
        }
    }
}
