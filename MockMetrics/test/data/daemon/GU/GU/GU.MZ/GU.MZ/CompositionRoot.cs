using System;
using System.Configuration;
using Autofac;
using Common.Types.Exceptions;
using GU.MZ.BL.Modules;
using GU.MZ.Import;
using GU.MZ.UI;
using GU.MZ.UI.Modules;

namespace GU.MZ
{
    internal class CompositionRoot
    {
        private IContainer _container;
        
        public void Register()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule(new InitModule());
                builder.RegisterModule(new MzBlModule(Convert.ToInt32(ConfigurationManager.AppSettings["ZlpAppType"])));
                builder.RegisterModule(new MzUiModule());
                builder.RegisterModule(new ImportModule());
                _container = builder.Build();

                builder = new ContainerBuilder();
                builder.RegisterInstance(_container);
                builder.Update(_container);
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка иницализации приложения", ex);
            }
        }

        public Starter Resolve()
        {
            return _container.Resolve<Starter>();
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