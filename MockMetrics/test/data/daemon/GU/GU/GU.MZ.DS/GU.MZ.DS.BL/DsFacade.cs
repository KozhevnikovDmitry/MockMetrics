using Common.BL;
using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;

namespace GU.MZ.DS.BL
{
    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class DsFacade
    {
        /// <summary>
        /// Инициализирует ядро бизнес-логики предметной области Лекарственное обеспечение.
        /// </summary>
        /// <exception cref="BLLException">Ошибка инициализация ядра бизнес-логики предметной области Лекарственное обеспечение</exception>
        public static void InitializeCore()
        {
            if (DsCore.Instance.InitializationException != null)
            {
                throw new BLLException("Ошибка инициализация ядра бизнес-логики предметной области Лекарственное обеспечение", DsCore.Instance.InitializationException);
            }
        }

        /// <summary>
        /// Возвращает флаг возможности подключения по конфигурации.
        /// </summary>
        /// <param name="config">Конфигурация подключения к базе данных</param>
        /// <returns>Флаг возможности подключения по конфигурации</returns>
        public static bool TestProviderConfiguration(IProviderConfiguration config)
        {
            return new DataAccessLayerInitializer().TestConfiguration(config);
        }

        /// <summary>
        /// Возвращает контейнер бизнес-логики предтменой области Лекарственное обеспечение
        /// </summary>
        /// <returns>Контейнер бизнес-логики предтменой области Лекарственное обеспечение</returns>
        public static IDomainLogicContainer GetLogicContainer()
        {
            return DsCore.Instance.DomainLogicContainer;
        }

        /// <summary>
        /// Возвращает <C>DsDictionaryManager</C> для предметной области Лекарственное обеспечение.
        /// </summary>
        /// <returns><C>DsDictionaryManager</C> для предметной области Лекарственное обеспечение</returns>
        public static DsDictionaryManager GetDictionaryManager()
        {
            return (DsDictionaryManager)DsCore.Instance.DictionaryManager;
        }
        
        /// <summary>
        /// Возвращает <C>UserPreferences</C> для предметной области Лекарственное обеспечение.
        /// </summary>
        /// <returns><C>UserPreferences</C> для предметной области Лекарственное обеспечение.</returns>
        public static DsUserPreferences GetUserPreferences()
        {
            return DsCore.Instance.UserPreferences as DsUserPreferences;
        }

        /// <summary>
        /// Возвращает маппер доменных объектов.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Маппер доменных объектов</returns>
        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return DsCore.Instance.DomainLogicContainer.ResolveDataMapper<T>();
        }

        /// <summary>
        /// Возвращает валидатор доменных объектов.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Маппер доменных объектов</returns>
        public static IDomainValidator<T> GetValidator<T>() where T : IPersistentObject
        {
            return DsCore.Instance.DomainLogicContainer.ResolveValidator<T>();
        }
    }
}
