using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Common.Types;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Module = Autofac.Module;

namespace GU.MZ.UI.Modules
{
    public class GenericVmModule : Module
    {
        private readonly IEnumerable<Assembly> _uiAssemblies;

        public GenericVmModule(IEnumerable<Assembly> uiAssemblies)
        {
            _uiAssemblies = uiAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterSmartValidateableVms(builder, _uiAssemblies);
            RegisterEntityInfoVms(builder, _uiAssemblies);
            RegisterSmartListVms(builder, _uiAssemblies);
            RegisterSmartListItemVms(builder, _uiAssemblies);
        }

        private void RegisterEntityInfoVms(ContainerBuilder builder, IEnumerable<Assembly> uiAssemblies)
        {
            builder.RegisterAssemblyTypes(uiAssemblies.ToArray())
                .Where(x => x.IsAssignableToGenericType(typeof(IEntityInfoVm<>)))
                .AsClosedTypesOf(typeof(IEntityInfoVm<>))
                .AsSelf();
        }

        private void RegisterSmartListVms(ContainerBuilder builder, IEnumerable<Assembly> uiAssemblies)
        {
            builder.RegisterAssemblyTypes(uiAssemblies.ToArray())
                .Where(x => x.IsAssignableToGenericType(typeof(ISmartListVm<>)))
                .AsClosedTypesOf(typeof(ISmartListVm<>))
                .AsSelf()
                .OnActivating(t => (t.Instance as ISmartListVm).SetUiFactory(t.Context.Resolve<IListVmUiFactory>()));
        }

        private void RegisterSmartListItemVms(ContainerBuilder builder, IEnumerable<Assembly> uiAssemblies)
        {
            builder.RegisterAssemblyTypes(uiAssemblies.ToArray())
                .Where(x => x.IsAssignableToGenericType(typeof(SmartListItemVm<>)))
                .AsClosedTypesOf(typeof(IListItemVM<>))
                .OnActivating(t => (t.Instance as ISmartValidateableVm).SetFacade(t.Context.Resolve<IValidateFacade>()));
        }

        private void RegisterSmartValidateableVms(ContainerBuilder builder, IEnumerable<Assembly> uiAssemblies)
        {
            builder.RegisterAssemblyTypes(uiAssemblies.ToArray())
                .Where(x => x.IsAssignableToGenericType(typeof(ISmartValidateableVm<>)) && !x.IsAssignableToGenericType(typeof(IListItemVM<>)))
                .AsClosedTypesOf(typeof(ISmartValidateableVm<>))
                .AsSelf()
                .OnActivating(t => (t.Instance as ISmartValidateableVm).SetFacade(t.Context.Resolve<IValidateFacade>()));
        }
    }
}