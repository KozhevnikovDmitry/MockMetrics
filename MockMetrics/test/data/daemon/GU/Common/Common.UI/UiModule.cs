using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Autofac;
using Common.DA.Interface;
using Common.Types;
using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent.EventSubscriber;
using Module = Autofac.Module;

namespace Common.UI
{
    public abstract class UiModule : Module
    {
        protected readonly IEnumerable<Assembly> _uiAssemblies;

        public UiModule(IEnumerable<Assembly> uiAssemblies)
        {
            _uiAssemblies = uiAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            CommonUiRegister(builder);
        }

        protected void CommonUiRegister(ContainerBuilder builder)
        {
            RegisterAllImplementsInterfase(builder, _uiAssemblies, typeof(IEditableVM<>));
            RegisterAllImplementsInterfase(builder, _uiAssemblies, typeof(IDomainObjectEventSubscriber<>));
            RegisterImplementsInterfase(builder, _uiAssemblies, typeof(IDomainValidateableVM<>), typeof(IListItemVM<>));

            RegisterImplementsInterfase(builder, _uiAssemblies, typeof(ISearchResultVM<>));
            RegisterImplementsInterfase(builder, _uiAssemblies, typeof(ISearchVM<>));
            RegisterImplementsInterfase(builder, _uiAssemblies, typeof(IListVM<>));
            RegisterImplementsInterfase(builder, _uiAssemblies, typeof(IListItemVM<>));

            builder.RegisterType<UiFactory>().AsSelf().AsImplementedInterfaces().SingleInstance();
        }
        
        protected void RegisterEditableView<TView, TEntity>(ContainerBuilder builder)
            where TView : UserControl
            where TEntity : IPersistentObject
        {
            builder.RegisterType<TView>().Named<UserControl>(typeof(TEntity).FullName);
        }

        protected void RegisterType<TInterface, TViewModel>(ContainerBuilder builder, string tag)
            where TInterface : INotifyPropertyChanged
            where TViewModel : INotifyPropertyChanged
        {
            builder.RegisterType<TViewModel>().Keyed<TInterface>(tag);
        }

        protected void RegisterImplementsInterfase(ContainerBuilder builder, IEnumerable<Assembly> assemblies, Type interfaceType)
        {
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                             .Where(x => x.IsAssignableToGenericType(interfaceType))
                             .AsClosedTypesOf(interfaceType).AsSelf();
        }

        protected void RegisterAllImplementsInterfase(ContainerBuilder builder, IEnumerable<Assembly> assemblies, Type interfaceType)
        {
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                             .Where(x => x.IsAssignableToGenericType(interfaceType))
                             .AsClosedTypesOf(interfaceType)
                             .AsSelf()
                             .AsImplementedInterfaces();
        }

        protected void RegisterImplementsInterfase(ContainerBuilder builder, IEnumerable<Assembly> assemblies, Type interfaceType, Type exceptType)
        {
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                             .Where(x => x.IsAssignableToGenericType(interfaceType) && !x.IsAssignableToGenericType(exceptType))
                             .AsClosedTypesOf(interfaceType);
        }
    }
}