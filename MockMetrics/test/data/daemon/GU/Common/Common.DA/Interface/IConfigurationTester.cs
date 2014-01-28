using Common.DA.ProviderConfiguration;

namespace Common.DA.Interface
{
    /// <summary>
    /// Интерфейс класса, ответсвенного за проверку подключения к базе данных
    /// </summary>
    public interface IConfigurationTester
    {
        /// <summary>
        /// Проверяет возможность поключения в БД по заданной конфигурации 
        /// </summary>
        /// <param name="config">Конфигурация подключения</param>
        /// <returns>Флаг возможности подключения</returns>
        bool TestConfiguration(IProviderConfiguration config);

        /// <summary>
        /// Проверяет возможность поключения в БД по заданной конфигурации 
        /// </summary>
        /// <param name="config">Конфигурация подключения</param>
        /// <returns>Информация о проверке подключения</returns>
        ConfigurationTestResult FullTestConfiguration(IProviderConfiguration config);
    }
}