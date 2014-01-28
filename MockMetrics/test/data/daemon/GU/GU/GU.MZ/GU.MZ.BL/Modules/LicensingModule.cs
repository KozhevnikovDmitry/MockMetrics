using Autofac;
using GU.MZ.BL.DomainLogic.Licensing;
using GU.MZ.BL.DomainLogic.Licensing.Renewal;

namespace GU.MZ.BL.Modules
{
    public class LicensingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LicenseProvider>().As<ILicenseProvider>();
            builder.RegisterType<LicenseObjectProvider>().As<ILicenseObjectProvider>();
            builder.RegisterType<LicenseRenewaller>().As<ILicenseRenewaller>().InstancePerLifetimeScope();

            builder.RegisterType<ReorganizationScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<RenameScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<NewWorksScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<MergeScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeNameWorkScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeOrgAddressScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeAddressScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<AnotherAddressScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<AnotherWorksScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<StopActivityAddressScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<StopWorksScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeDocumentScenario>().As<IRenewalScenario>().InstancePerLifetimeScope();
        }
    }
}
