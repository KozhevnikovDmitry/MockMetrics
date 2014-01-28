using Autofac;

using Moq;

using SpecManager.BL.Interface;
using SpecManager.BL.Model;
using SpecManager.BL.SpecSource;
using SpecManager.DA;

namespace SpecManager.BL.Test.Integration
{
    public class TestCompositionRoot
    {
        public const string ConnectionString = "Server=172.25.253.154;Port=5432;Database=gosus;User Id=gosus;Password=gosus;";

        public IDomainDbManager TestDbManager;

        public IContainer Container { get; private set; }

        private readonly IDbFactory _realDbFactory;

        public TestCompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DaModule>();
            builder.RegisterModule<BlModule>();
            this.Container = builder.Build();

            this._realDbFactory = Container.Resolve<IDbFactory>();
            this._realDbFactory.ConnectionString = ConnectionString;

            builder = new ContainerBuilder();
            builder.RegisterInstance(GetDbFactory()).As<IDbFactory>();
            builder.Update(Container);
        }

        public void SetTransaction()
        {
            TestDbManager = new TestDbManager(this._realDbFactory.GetDbManager());
            TestDbManager.BeginDomainTransaction();
        }

        public void RollbackTransaction()
        {
            TestDbManager.RollbackDomainTransaction();
            TestDbManager.Dispose();
        }

        public IDomainDataMapper<T> GetDataMapper<T>() where T : IDomainObject
        {
            var dataMapper = Container.Resolve<IDomainDataMapper<T>>();
            return dataMapper;
        }

        public IDbFactory GetDbFactory()
        {
            var mock = new Mock<IDbFactory>();
            mock.Setup(t => t.GetDbManager()).Returns(() => TestDbManager);
            return mock.Object;
        }

        public DbSpecSource GetDbSpecSource(int id)
        {
            return new DbSpecSource(id, this.GetDataMapper<Spec>(), GetDbFactory());
        }


    }
}
