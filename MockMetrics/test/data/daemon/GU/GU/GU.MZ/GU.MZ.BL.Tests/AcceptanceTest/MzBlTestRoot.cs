using System;
using System.Configuration;
using Autofac;
using Common.BL.Authentification;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.Enisey.BL;
using GU.MZ.BL.DomainLogic;
using GU.MZ.BL.Modules;
using GU.MZ.Import;

namespace GU.MZ.BL.Tests.AcceptanceTest
{
    public interface ITestRoot
    {
        void Register();
        MzTestBlFactory Resolve();
        void Release();
        void BeginTransactionTest();
        void EndTransactionTest();
    }

    public class MzBlTestRoot : ITestRoot
    {
        private readonly bool _isTransactional;
        private IContainer _container;
        private TestDbManager db;

        public MzBlTestRoot(bool isTransactional)
        {
            _isTransactional = isTransactional;
        }

        public virtual void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MzTestModule>();
            builder.RegisterModule(new MzBlModule(Convert.ToInt32(ConfigurationManager.AppSettings["ZlpAppType"])));
            builder.RegisterModule<EniseyModule>();
            builder.RegisterModule<ImportModule>();
            _container = builder.Build();
            var auth = _container.Resolve<IAuthentificator>();
            auth.AuthentificateUser("test_mz_lic", "test");

            if (_isTransactional)
            {
                RegisterTransactional();
            }

            builder = new ContainerBuilder();
            builder.RegisterInstance(_container);
            builder.Update(_container);
        }

        private void RegisterTransactional()
        {
            db = new TestDbManager();
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new StubDbCtx(db)).As<IDomainContext>();
            builder.RegisterInstance(db).As<IDomainDbManager>().AsSelf();
            builder.RegisterInstance<Func<IDomainDbManager>>(() => db);
            builder.Update(_container);
        }

        public MzTestBlFactory Resolve()
        {
            return _container.Resolve<MzTestBlFactory>();
        }

        public void Release()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        public void BeginTransactionTest()
        {
            db.ChangeDbManager();
            db.BeginDomainTransaction();
        }

        public void EndTransactionTest()
        {
            db.RollbackDomainTransaction();
            db.ReallyDispose();
        }
    }
}
