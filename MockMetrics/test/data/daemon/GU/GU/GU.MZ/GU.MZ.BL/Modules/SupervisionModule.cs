using Autofac;
using GU.MZ.BL.DomainLogic.GrantResult;
using GU.MZ.BL.DomainLogic.Inspect;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Notify;
using GU.MZ.BL.DomainLogic.Order;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.BL.DomainLogic.ViolationResolve;

namespace GU.MZ.BL.Modules
{
    public class SupervisionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SupervisionFacade>().InstancePerLifetimeScope();
            builder.RegisterType<LicenseHolderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LicenseDossierRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DossierFileNotifier>().InstancePerLifetimeScope();
            builder.RegisterType<OrderProvider>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentExpert>().InstancePerLifetimeScope();
            builder.RegisterType<HolderInspecter>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceResultGranter>().InstancePerLifetimeScope();
            builder.RegisterType<ViolationResolver>().InstancePerLifetimeScope();
        }
    }
}
