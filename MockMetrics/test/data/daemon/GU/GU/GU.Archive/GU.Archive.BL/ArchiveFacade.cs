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

using GU.Archive.BL.Reporting.Mapping;

namespace GU.Archive.BL
{

    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class ArchiveFacade
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
                ArchiveCore.Instance.Initialize(config);
                if (ArchiveCore.Instance.InitializationException != null)
                {
                    throw new BLLException("Failed to initialize BLL", ArchiveCore.Instance.InitializationException);
                }
            }
        }

        public static ArchiveDictionaryManager GetDictionaryManager()
        {
            return (ArchiveDictionaryManager)ArchiveCore.Instance.DictionaryManager;
        }

        public static ArchiveUserPreferences GetUserPreferences()
        {
            return ArchiveCore.Instance.UserPreferences as ArchiveUserPreferences;
        }

        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return ArchiveCore.Instance.ArchiveLogicContainer.ResolveDataMapper<T>();
        }

        public static IDomainLogicContainer GetLogicContainer()
        {
            return ArchiveCore.Instance.ArchiveLogicContainer;
        }

        public static TaskPostStatReport GetTaskPostStatReport()
        {
            return ArchiveCore.Instance.ArchiveLogicContainer.ResolveTaskPostStatReport();
        }
    }
}
