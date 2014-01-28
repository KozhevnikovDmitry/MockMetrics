using System.Configuration;
using Autofac;
using Common.BL;
using Common.DA;
using Common.DA.ProviderConfiguration;
using GU.MZ.BL.DomainLogic.Authentification;
using GU.MZ.BL.Tests.AcceptanceTest;

namespace GU.MZ.BL.Tests
{
    public class MzTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var с = ConfigurationManager.AppSettings["ConnectionConfig"];
            builder.Register(c => new ProviderConfigurationFactory()
                   .GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"]))
                   .As<IProviderConfiguration>()
                   .SingleInstance();
            builder.RegisterType<MzAutentificator>().AsImplementedInterfaces();
            builder.RegisterType<ConnectChecker>();
            builder.RegisterType<ContentParser>();
            builder.RegisterType<DataAccessLayerInitializer>().AsImplementedInterfaces();
            builder.RegisterType<MzTestBlFactory>().AsSelf().SingleInstance();
        }
    }
}
