using Autofac;

using SpecManager.BL.Interface;
using SpecManager.BL.Model;
using SpecManager.DA.DataMapping;
using SpecManager.DA.DbManagement;

namespace SpecManager.DA
{
    public class DaModule : Module  
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SpecDataMapper>().As<IDomainDataMapper<Spec>>();
            builder.RegisterType<SpecNodeDataMapper>().As<IDomainDataMapper<SpecNode>>();
            builder.RegisterType<DictDataMapper>().As<IDomainDataMapper<Dict>>();
            builder.RegisterType<DomainDbManager>().As<IDomainDbManager>();
            builder.RegisterType<DbFactory>().As<IDbFactory>();
        }
    }
}
