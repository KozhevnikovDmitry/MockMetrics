using Autofac;
using GU.MZ.BL.DomainLogic.Linkage;

namespace GU.MZ.BL.Modules
{
    public class LinkageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DossierFileRepository>();
            builder.RegisterType<Linkager>().As<ILinkager>().InstancePerLifetimeScope();
            builder.RegisterType<LinkageHolderAddIn>().As<ILinkagerAddIn>().InstancePerLifetimeScope();
            builder.RegisterType<LinkageDossierAddIn>().As<ILinkagerAddIn>().InstancePerLifetimeScope();
            builder.RegisterType<LinkageRequisitesAddIn>().As<ILinkagerAddIn>().InstancePerLifetimeScope();
            builder.RegisterType<LinkageLicenseAddIn>().As<ILinkagerAddIn>().InstancePerLifetimeScope();
            builder.RegisterType<CheckHolderDataAddIn>().As<ILinkagerAddIn>().InstancePerLifetimeScope();
            builder.RegisterType<SetupFileAddIn>().As<ILinkagerAddIn>().InstancePerLifetimeScope();
        }
    }
}
