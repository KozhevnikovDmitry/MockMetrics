using System;

using Common.BL;
using Common.BL.DataMapping;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;

namespace GU.MZ.BL
{
    [Obsolete]
    public static class GumzFacade
    {
        private static readonly object _locker = new object();

        public static void InitializeCore(IDomainLogicContainer blFactory)
        {
            lock (_locker)
            {
                GumzCore.Instance.Initialize(blFactory);
                if (GumzCore.Instance.InitializationException != null)
                {
                    throw new BLLException("Ошибка инициализация ядра бизнес-логики предметной области Лицензирование", GumzCore.Instance.InitializationException);
                }
            }
        }

        public static bool TestProviderConfiguration(IProviderConfiguration config)
        {
            return new DataAccessLayerInitializer().TestConfiguration(config);
        }

        public static IDomainLogicContainer GetLogicContainer()
        {
            return GumzCore.Instance.MzBlFactory;
        }

        public static GumzDictionaryManager GetDictionaryManager()
        {
            return (GumzDictionaryManager)GumzCore.Instance.DictionaryManager;
        }
        
        public static GumzUserPreferences GetUserPreferences()
        {
            return GumzCore.Instance.UserPreferences as GumzUserPreferences;
        }

        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return GumzCore.Instance.MzBlFactory.ResolveDataMapper<T>();
        }

        public static IDomainValidator<T> GetValidator<T>() where T : IPersistentObject
        {
            return GumzCore.Instance.MzBlFactory.ResolveValidator<T>();
        }
    }
}
