using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.BL.Validation;
using Common.Types;
using Module = Autofac.Module;

namespace Common.BL
{
    public class BlModule : Module
    {
        protected readonly IEnumerable<Assembly> DmAssemblies;
        protected readonly Assembly[] BlAssemblies;

        public BlModule(IEnumerable<Assembly> blAssemblies, IEnumerable<Assembly> dmAssemblies)
        {
            DmAssemblies = dmAssemblies;
            BlAssemblies = blAssemblies.ToArray();
        }

        protected override void Load(ContainerBuilder builder)
        {
            CommonBlRegister(builder);
        }

        protected void CommonBlRegister(ContainerBuilder builder)
        {
            builder.RegisterInstance<IDomainContext>(new DomainContext.DomainContext(BlAssemblies));

            foreach (var assembly in BlAssemblies)
            {
                builder.RegisterAssemblyTypes(assembly)
                       .Where(x => x.IsAssignableToGenericType(typeof(IDomainValidator<>)))
                       .AsImplementedInterfaces()
                       .SingleInstance();

                builder.RegisterAssemblyTypes(assembly)
                                .Where(x => x.IsAssignableToGenericType(typeof(IDomainDataMapper<>)))
                                .WithProperty("DomainKey", assembly.FullName)
                                .PropertiesAutowired(PropertyWiringFlags.AllowCircularDependencies)
                                .AsImplementedInterfaces()
                                .SingleInstance();

                builder.RegisterAssemblyTypes(assembly)
                                .Where(x => x.IsAssignableToGenericType(typeof(ISearchStrategy<>)))
                                .WithProperty("DomainKey", assembly.FullName)
                                .WithProperty("Searcher", Activator.CreateInstance(assembly.GetTypes().Single(t => typeof(IDomainObjectSearcher).IsAssignableFrom(t))))
                                .AsImplementedInterfaces();
            }

            builder.RegisterType<BlFactory>()
                   .AsSelf()
                   .As<IDomainLogicContainer>()
                   .SingleInstance();
        }

        protected void RegisterSearchProperties(ContainerBuilder builder, IEnumerable<IUserPreferences> preferenceses)
        {
            var searchSpecificationContainer = new SearchSpecificationContainer() {Marker = "NSHS"};
            searchSpecificationContainer.RegisterSearchProperties(DmAssemblies);
            foreach (var userPreferencese in preferenceses)
            {
                searchSpecificationContainer.RegisterSearchPresetList(userPreferencese.SearchPresetSpecContainer.PresetSpecList);
            }
            builder.RegisterInstance(searchSpecificationContainer).AsSelf().AsImplementedInterfaces();
        }
    }
}
