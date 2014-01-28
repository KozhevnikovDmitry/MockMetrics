using Autofac;
using GU.MZ.BL.Reporting;
using GU.MZ.BL.Reporting.Mapping;

namespace GU.MZ.BL.Modules
{
    public class MzReportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LicenseBlankReport>();
            builder.RegisterType<StatementByServiceReport>();
            builder.RegisterType<LicenseByActivityReport>();
            builder.RegisterType<FullActivityDataReport>();
            builder.RegisterType<ViolationNoticeReport>();
            builder.RegisterType<StandartOrderReport>();
            builder.RegisterType<ExpertiseOrderReport>();
            builder.RegisterType<InspectionOrderReport>();
            builder.RegisterType<FullActivityDataReport>();
            builder.RegisterType<InventoryReport>().InstancePerLifetimeScope();
            builder.RegisterType<MzReportPool>().AsImplementedInterfaces();
        }
    }
}
