using Autofac;

namespace GU.MZ.Import
{
    public class ImportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImportSession>();
            builder.RegisterType<Importer>();
            builder.RegisterType<Synchronizer>();
            builder.RegisterType<Protocoller>().WithParameter(new NamedParameter("path", "log.xml"));
        }
    }
}
