using System.Windows.Controls;

using Autofac;

using SpecManager.BL;
using SpecManager.BL.Interface;
using SpecManager.BL.Model;
using SpecManager.BL.SpecSource;
using SpecManager.DA;
using SpecManager.UI;
using SpecManager.UI.ViewModel;
using SpecManager.UI.ViewModel.SpecViewModel;

namespace SpecManager
{
    public class CompositionRoot : ISpecViewFactory, ISpecSourceFactory
    {
        private readonly IContainer _iocContainer;

        public CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DaModule>();
            builder.RegisterModule<BlModule>();
            builder.RegisterModule<UiModule>();
            _iocContainer = builder.Build();

            builder = new ContainerBuilder();
            builder.RegisterInstance(this).AsImplementedInterfaces();
            builder.Update(_iocContainer);
        }

        internal T Resolve<T>()
        {
            return this._iocContainer.Resolve<T>();
        }
        
        #region ISpecViewFactory

        public SpecVmBase GetSpecVm(SpecNodeParent specNodeParent)
        {
            return _iocContainer.ResolveKeyed<SpecVmBase>(specNodeParent.GetType(), new NamedParameter("specItem", specNodeParent));
        }

        public UserControl GetSpecView(SpecNodeParent specNodeParent)
        {
            return _iocContainer.ResolveKeyed<UserControl>(specNodeParent.GetType());
        }

        public SpecDockableVm GetSpecDockableVm(Spec spec, string path)
        {
            var specSource = GetSpecSource(path);
            return new SpecDockableVm(spec, _iocContainer.Resolve<SpecTreeClipboard>(), specSource, this);
        }

        public SpecDockableVm GetSpecDockableVm(string path)
        {
            var specSource = GetSpecSource(path);
            return new SpecDockableVm(specSource.Get(), _iocContainer.Resolve<SpecTreeClipboard>(), specSource, this);

        }

        public SpecDockableVm GetSpecDockableVm(int id, string connectionString)
        {
            var specSource = GetSpecSource(id, connectionString);
            return new SpecDockableVm(specSource.Get(), _iocContainer.Resolve<SpecTreeClipboard>(), specSource, this);
        }

        public SpecDockableVm GetSpecDockableVm(string uri, string connectionString)
        {
            var specSource = GetSpecSource(uri, connectionString);
            return new SpecDockableVm(specSource.Get(), _iocContainer.Resolve<SpecTreeClipboard>(), specSource, this);
        }

        #endregion

        #region ISpecSourceFactory

        public ISpecSource GetSpecSource(string filePath)
        {
            return new XmlSpecSource(filePath);
        }

        public ISpecSource GetSpecSource(int id, string connectionString)
        {
            var dbFactory = _iocContainer.Resolve<IDbFactory>();
            dbFactory.ConnectionString = connectionString;
            return new DbSpecSource(id, GetDataMapper<Spec>(connectionString), dbFactory);
        }

        public ISpecSource GetSpecSource(string uri, string connectionString)
        {
            var dbFactory = _iocContainer.Resolve<IDbFactory>();
            dbFactory.ConnectionString = connectionString;
            return new DbSpecSource(uri, GetDataMapper<Spec>(connectionString), dbFactory);

        }

        private IDomainDataMapper<T> GetDataMapper<T>(string connectionString) where T : IDomainObject
        {
            var dbFactory = _iocContainer.Resolve<IDbFactory>();
            dbFactory.ConnectionString = connectionString;
            var dataMapper = _iocContainer.Resolve<IDomainDataMapper<T>>(new NamedParameter("dbFactory", dbFactory));
            return dataMapper;
        }
        
        #endregion
    }
}
