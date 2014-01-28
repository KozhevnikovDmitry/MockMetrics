using Common.BL;
using Common.BL.DataMapping;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;
using GU.Trud.BL.Export.Interface;

namespace GU.Trud.BL
{
    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class TrudFacade
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
                TrudCore.Instance.Initialize(config);
                if (TrudCore.Instance.InitializationException != null)
                {
                    throw new BLLException("Failed to initialize BLL", TrudCore.Instance.InitializationException);
                }
            }
        }

        public static TrudDictionaryManager GetDictionaryManager()
        {
            return (TrudDictionaryManager)TrudCore.Instance.DictionaryManager;
        }

        public static TrudUserPreferences GetUserPreferences()
        {
            return TrudCore.Instance.UserPreferences as TrudUserPreferences;
        }

        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return TrudCore.Instance.TrudLogicContainer.ResolveDataMapper<T>();
        }

        public static IGenerateExportService GetExportService(string operatingCatalog)
        {
            return TrudCore.Instance.TrudLogicContainer.ResolveExportService(operatingCatalog);
        }

        public static ISendExportService GetSendExportService(string operatingCatalog)
        {
            return TrudCore.Instance.TrudLogicContainer.ResolveSendExportService(operatingCatalog);
        }

        public static ISaveExportService GetSaveExportService()
        {
            return TrudCore.Instance.TrudLogicContainer.ResolveSaveExportService();
        }

        public static IDomainLogicContainer GetLogicContainer()
        {
            return TrudCore.Instance.TrudLogicContainer;
        }
    }
}
