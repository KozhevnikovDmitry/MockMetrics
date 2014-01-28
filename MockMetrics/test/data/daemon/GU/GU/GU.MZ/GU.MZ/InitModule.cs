using System;
using System.Configuration;
using Autofac;
using Common.BL;
using Common.DA;
using Common.DA.ProviderConfiguration;
using Common.UI;
using Common.UI.ViewModel;
using GU.MZ.BL.DomainLogic.Authentification;
using GU.MZ.UI.ViewModel;

namespace GU.MZ
{
    internal class InitModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ProviderConfigurationFactory()
                .GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"])).SingleInstance();
            builder.RegisterType<MzAutentificator>().AsImplementedInterfaces();
            builder.RegisterType<ConnectChecker>();
            builder.RegisterType<LoginVM>();
            builder.RegisterType<MainVm>();
            builder.RegisterType<Lazy<LoginVM>>();
            builder.RegisterType<Lazy<MainVm>>();
            builder.RegisterType<DataAccessLayerInitializer>().AsImplementedInterfaces();
            builder.RegisterType<SplashService>();
            builder.RegisterType<Starter>();
        }
    }
}