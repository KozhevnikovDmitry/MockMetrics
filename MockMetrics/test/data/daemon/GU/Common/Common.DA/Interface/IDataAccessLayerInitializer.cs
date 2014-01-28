using System.Collections.Generic;
using System.Reflection;

using Common.DA.ProviderConfiguration;

namespace Common.DA.Interface
{
    /// <summary>
    /// Интерфейс класса, ответсвенного за инициализацию базы данных
    /// </summary>
    public interface IDataAccessLayerInitializer : IConfigurationTester
    {

        /// <summary>
        /// Возвращает менеджер базы данных, до которой удалось достучаться.
        /// </summary>
        /// <returns>Менеджер БД</returns>
        IDomainDbManager GetDbManager(Assembly blAssembly);

        /// <summary>
        /// Инициализирует слой доступа к данным.
        /// </summary>
        /// <param name="config">Конфигурация подключения</param>
        /// <param name="dataModelAssembly">Сборка с дата классами</param>
        /// <param name="domainObjectInitilizer">Инициализатор доменных объектов</param>
        void Initialize(IProviderConfiguration config, 
                        Assembly dataModelAssembly, 
                        IDomainObjectInitializer domainObjectInitilizer);

        void Initialize(IProviderConfiguration config,
            Dictionary<Assembly,IDomainObjectInitializer> initDictionary);
    }
}