using Autofac;
using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.Modules
{
    internal class GuParseModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MedParser>().As<IParserImpl>();
            builder.RegisterType<DrugParser>().As<IParserImpl>();
            builder.RegisterType<FarmParser>().As<IParserImpl>();
            builder.RegisterType<ParserFacade>().As<IParser>();
            builder.RegisterType<ContentMapper>().As<IContentMapper>();
        }
    }
}
