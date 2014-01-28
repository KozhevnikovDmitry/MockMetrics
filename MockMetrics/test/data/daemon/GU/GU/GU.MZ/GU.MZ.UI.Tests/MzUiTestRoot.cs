using System;
using System.Configuration;
using Autofac;
using Common.BL.Authentification;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.BL.Modules;
using GU.MZ.BL.Tests;
using GU.MZ.BL.Tests.AcceptanceTest;
using GU.MZ.UI.Modules;

namespace GU.MZ.UI.Tests
{
    public class MzUiTestRoot
    {
        private readonly bool _isTransactional;
        private IContainer _container;

        public MzUiTestRoot(bool isTransactional)
        {
            _isTransactional = isTransactional;
        }

        public virtual void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MzTestModule>();
            builder.RegisterModule(new MzBlModule(Convert.ToInt32(ConfigurationManager.AppSettings["ZlpAppType"])));
            builder.RegisterModule<MzUiModule>();
            if (_isTransactional)
            {
                RegisterTransactional(builder);
            }
            _container = builder.Build();
            builder = new ContainerBuilder();
            builder.RegisterInstance(_container);
            builder.Update(_container);
        }

        private void RegisterTransactional(ContainerBuilder builder)
        {
            builder.RegisterType<StubDbCtx>().As<IDomainContext>().SingleInstance();
            builder.Register(c => new TestDbManager()).As<IDomainDbManager>().SingleInstance();
        }

        public IContainer Resolve()
        {
            var auth = _container.Resolve<IAuthentificator>();
            auth.AuthentificateUser("test_mz_lic", "test");
            return _container;
        }

        public void Release()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }
    }
}
