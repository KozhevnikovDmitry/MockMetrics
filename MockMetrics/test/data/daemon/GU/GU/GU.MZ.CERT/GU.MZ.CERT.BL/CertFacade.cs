using Common.BL;
using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;

namespace GU.MZ.CERT.BL
{
    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class CertFacade
    {
        /// <summary>
        /// Инициализирует ядро бизнес-логики предметной области Аттестация.
        /// </summary>
        /// <exception cref="BLLException">Ошибка инициализация ядра бизнес-логики предметной области Аттестация</exception>
        public static void InitializeCore()
        {
            if (CertCore.Instance.InitializationException != null)
            {
                throw new BLLException("Ошибка инициализация ядра бизнес-логики предметной области Аттестация", CertCore.Instance.InitializationException);
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
        /// Возвращает контейнер бизнес-логики предтменой области Аттестация
        /// </summary>
        /// <returns>Контейнер бизнес-логики предтменой области Аттестация</returns>
        public static IDomainLogicContainer GetLogicContainer()
        {
            return CertCore.Instance.DomainLogicContainer;
        }

        /// <summary>
        /// Возвращает <C>CertDictionaryManager</C> для предметной области Аттестация.
        /// </summary>
        /// <returns><C>CertDictionaryManager</C> для предметной области Аттестация</returns>
        public static CertDictionaryManager GetDictionaryManager()
        {
            return (CertDictionaryManager)CertCore.Instance.DictionaryManager;
        }
        
        /// <summary>
        /// Возвращает <C>UserPreferences</C> для предметной области Аттестация.
        /// </summary>
        /// <returns><C>UserPreferences</C> для предметной области Аттестация.</returns>
        public static CertUserPreferences GetUserPreferences()
        {
            return CertCore.Instance.UserPreferences as CertUserPreferences;
        }

        /// <summary>
        /// Возвращает маппер доменных объектов.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Маппер доменных объектов</returns>
        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return CertCore.Instance.DomainLogicContainer.ResolveDataMapper<T>();
        }

        /// <summary>
        /// Возвращает валидатор доменных объектов.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Маппер доменных объектов</returns>
        public static IDomainValidator<T> GetValidator<T>() where T : IPersistentObject
        {
            return CertCore.Instance.DomainLogicContainer.ResolveValidator<T>();
        }
    }
}
