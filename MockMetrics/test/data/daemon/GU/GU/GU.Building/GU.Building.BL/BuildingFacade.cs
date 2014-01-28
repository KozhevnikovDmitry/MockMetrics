using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL;
using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;

namespace GU.Building.BL
{

    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class BuildingFacade
    {
        public static bool TestProviderConfiguration(IProviderConfiguration config)
        {
            return new DataAccessLayerInitializer().TestConfiguration(config);
        }

        private static readonly object _locker = new object();
        public static void InitializeCore(IProviderConfiguration config)
        {
            lock (_locker)
            {
                BuildingCore.Instance.Initialize(config);
                if (BuildingCore.Instance.InitializationException != null)
                {
                    throw new BLLException("Failed to initialize BLL", BuildingCore.Instance.InitializationException);
                }
            }
        }

        public static BuildingDictionaryManager GetDictionaryManager()
        {
            return (BuildingDictionaryManager)BuildingCore.Instance.DictionaryManager;
        }

        public static BuildingUserPreferences GetUserPreferences()
        {
            return BuildingCore.Instance.UserPreferences as BuildingUserPreferences;
        }

        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return BuildingCore.Instance.DomainLogicContainer.ResolveDataMapper<T>();
        }

        public static IDomainLogicContainer GetLogicContainer()
        {
            return BuildingCore.Instance.DomainLogicContainer;
        }
    }
}
