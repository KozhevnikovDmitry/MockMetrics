using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DA.ProviderConfiguration;
using Common.DA;
using Common.Types.Exceptions;
using GU.BL;
using System.Configuration;
using GU.Enisey.BL.Converters;

namespace GU.Enisey.BL
{
    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class EniseyFacade
    {
        public static bool TestProviderConfiguration(IProviderConfiguration config)
        {
            return new DataAccessLayerInitializer().TestConfiguration(config);
        }

        private static readonly object _locker = new object();

        public static void Initialize()
        {
            Initialize(null);
        }

        public static void Initialize(IProviderConfiguration config)
        {
            lock (_locker)
            {
                if (config == null)
                {
                    ProviderConfigurationFactory confFactory = new ProviderConfigurationFactory();
                    config = confFactory.GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"]);
                    config.User = "guenisey";
                    config.Password = "wAll88fOamY&";
                }

                GuFacade.InitializeCore(config);
                EniseyFacade.InitializeCore(config);
            }
        }

        private static void InitializeCore(IProviderConfiguration config)
        {
            EniseyCore.Instance.Initialize(config);
            if (EniseyCore.Instance.InitializationException != null)
            {
                throw new BLLException("Failed to initialize BLL", EniseyCore.Instance.InitializationException);
            }
        }

        public static void InitializeServices()
        {
            EniseyCore.Instance.InitializeServices();
        }

        public static EniseyDbManager GetDbManager()
        {
            return new EniseyDbManager();
        }

        public static ConverterManager GetConverterManager()
        {
            return EniseyCore.Instance.GetConverterManager();
        }
    }
}
