using Autofac;

using SpecManager.BL.Connect;
using SpecManager.BL.SpecSource;

namespace SpecManager.BL
{
    public class BlModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbConnector>();
            builder.RegisterType<DbSpecSource>().AsImplementedInterfaces();
            builder.RegisterType<XmlSpecSource>().AsImplementedInterfaces();
        }
    }
}
