using System.Windows.Controls;
using Autofac;
using GU.MZ.UI.View.SupervisionView;
using GU.MZ.UI.ViewModel.SupervisionViewModel;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Interface;

namespace GU.MZ.UI.Modules
{
    public class SupervisionVmModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Register Supervision Views

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("1");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("2");
            builder.RegisterType<NoticeViolationStepView>().Named<UserControl>("3");
            builder.RegisterType<OrderAcceptOrRejectStepView>().Named<UserControl>("4");
            builder.RegisterType<OrderExpertiseStepView>().Named<UserControl>("5");
            builder.RegisterType<ExpertiseStepView>().Named<UserControl>("6");
            builder.RegisterType<OrderInspectionStepView>().Named<UserControl>("7");
            builder.RegisterType<InspectionStepView>().Named<UserControl>("8");
            builder.RegisterType<OrderGrantLicenseStepView>().Named<UserControl>("9");
            builder.RegisterType<GrantNewLicenseStepView>().Named<UserControl>("10");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("11");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("12");
            builder.RegisterType<NoticeViolationStepView>().Named<UserControl>("13");
            builder.RegisterType<OrderAcceptOrRejectStepView>().Named<UserControl>("14");
            builder.RegisterType<OrderExpertiseStepView>().Named<UserControl>("15");
            builder.RegisterType<ExpertiseStepView>().Named<UserControl>("16");
            builder.RegisterType<OrderInspectionStepView>().Named<UserControl>("17");
            builder.RegisterType<InspectionStepView>().Named<UserControl>("18");
            builder.RegisterType<OrderRenewalLicenseStepView>().Named<UserControl>("19");
            builder.RegisterType<RenewalLicenseStepView>().Named<UserControl>("20");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("21");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("22");
            builder.RegisterType<OrderStopLicenseStepView>().Named<UserControl>("23");
            builder.RegisterType<StopLicenseStepView>().Named<UserControl>("24");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("25");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("26");
            builder.RegisterType<StubWorkingStepView>().Named<UserControl>("27");
            builder.RegisterType<GrantResultStepView>().Named<UserControl>("28");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("29");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("30");
            builder.RegisterType<StubWorkingStepView>().Named<UserControl>("31");
            builder.RegisterType<GrantResultStepView>().Named<UserControl>("32");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("33");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("34");
            builder.RegisterType<NoticeViolationStepView>().Named<UserControl>("35");
            builder.RegisterType<OrderAcceptOrRejectStepView>().Named<UserControl>("36");
            builder.RegisterType<OrderExpertiseStepView>().Named<UserControl>("37");
            builder.RegisterType<ExpertiseStepView>().Named<UserControl>("38");
            builder.RegisterType<OrderInspectionStepView>().Named<UserControl>("39");
            builder.RegisterType<InspectionStepView>().Named<UserControl>("40");
            builder.RegisterType<OrderGrantLicenseStepView>().Named<UserControl>("41");
            builder.RegisterType<GrantNewLicenseStepView>().Named<UserControl>("42");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("43");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("44");
            builder.RegisterType<NoticeViolationStepView>().Named<UserControl>("45");
            builder.RegisterType<OrderAcceptOrRejectStepView>().Named<UserControl>("46");
            builder.RegisterType<OrderExpertiseStepView>().Named<UserControl>("47");
            builder.RegisterType<ExpertiseStepView>().Named<UserControl>("48");
            builder.RegisterType<OrderInspectionStepView>().Named<UserControl>("49");
            builder.RegisterType<InspectionStepView>().Named<UserControl>("50");
            builder.RegisterType<OrderRenewalLicenseStepView>().Named<UserControl>("51");
            builder.RegisterType<RenewalLicenseStepView>().Named<UserControl>("52");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("53");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("54");
            builder.RegisterType<OrderStopLicenseStepView>().Named<UserControl>("55");
            builder.RegisterType<StopLicenseStepView>().Named<UserControl>("56");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("57");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("58");
            builder.RegisterType<StubWorkingStepView>().Named<UserControl>("59");
            builder.RegisterType<GrantResultStepView>().Named<UserControl>("60");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("61");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("62");
            builder.RegisterType<StubWorkingStepView>().Named<UserControl>("63");
            builder.RegisterType<GrantResultStepView>().Named<UserControl>("64");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("65");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("66");
            builder.RegisterType<NoticeViolationStepView>().Named<UserControl>("67");
            builder.RegisterType<OrderAcceptOrRejectStepView>().Named<UserControl>("68");
            builder.RegisterType<OrderExpertiseStepView>().Named<UserControl>("69");
            builder.RegisterType<ExpertiseStepView>().Named<UserControl>("70");
            builder.RegisterType<OrderInspectionStepView>().Named<UserControl>("71");
            builder.RegisterType<InspectionStepView>().Named<UserControl>("72");
            builder.RegisterType<OrderGrantLicenseStepView>().Named<UserControl>("73");
            builder.RegisterType<GrantNewLicenseStepView>().Named<UserControl>("74");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("75");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("76");
            builder.RegisterType<NoticeViolationStepView>().Named<UserControl>("77");
            builder.RegisterType<OrderAcceptOrRejectStepView>().Named<UserControl>("78");
            builder.RegisterType<OrderExpertiseStepView>().Named<UserControl>("79");
            builder.RegisterType<ExpertiseStepView>().Named<UserControl>("80");
            builder.RegisterType<OrderInspectionStepView>().Named<UserControl>("81");
            builder.RegisterType<InspectionStepView>().Named<UserControl>("82");
            builder.RegisterType<OrderRenewalLicenseStepView>().Named<UserControl>("83");
            builder.RegisterType<RenewalLicenseStepView>().Named<UserControl>("84");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("85");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("86");
            builder.RegisterType<OrderStopLicenseStepView>().Named<UserControl>("87");
            builder.RegisterType<StopLicenseStepView>().Named<UserControl>("88");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("89");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("90");
            builder.RegisterType<StubWorkingStepView>().Named<UserControl>("91");
            builder.RegisterType<GrantResultStepView>().Named<UserControl>("92");

            builder.RegisterType<ResponsibleEmployeeStepView>().Named<UserControl>("93");
            builder.RegisterType<LinkageStepView>().Named<UserControl>("94");
            builder.RegisterType<StubWorkingStepView>().Named<UserControl>("95");
            builder.RegisterType<GrantResultStepView>().Named<UserControl>("96");

            #endregion

            #region Register Supervision ViewModels

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("1");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("2");
            builder.RegisterType<NoticeViolationStepVm>().Named<ISupervisionStepVm>("3");
            builder.RegisterType<OrderAcceptOrRejectStepVm>().Named<ISupervisionStepVm>("4");
            builder.RegisterType<OrderExpertiseStepVm>().Named<ISupervisionStepVm>("5");
            builder.RegisterType<ExpertiseStepVm>().Named<ISupervisionStepVm>("6");
            builder.RegisterType<OrderInspectionStepVm>().Named<ISupervisionStepVm>("7");
            builder.RegisterType<InspectionStepVm>().Named<ISupervisionStepVm>("8");
            builder.RegisterType<OrderGrantLicenseStepVm>().Named<ISupervisionStepVm>("9");
            builder.RegisterType<GrantNewLicenseStepVm>().Named<ISupervisionStepVm>("10");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("11");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("12");
            builder.RegisterType<NoticeViolationStepVm>().Named<ISupervisionStepVm>("13");
            builder.RegisterType<OrderAcceptOrRejectStepVm>().Named<ISupervisionStepVm>("14");
            builder.RegisterType<OrderExpertiseStepVm>().Named<ISupervisionStepVm>("15");
            builder.RegisterType<ExpertiseStepVm>().Named<ISupervisionStepVm>("16");
            builder.RegisterType<OrderInspectionStepVm>().Named<ISupervisionStepVm>("17");
            builder.RegisterType<InspectionStepVm>().Named<ISupervisionStepVm>("18");
            builder.RegisterType<OrderRenewalLicenseStepVm>().Named<ISupervisionStepVm>("19");
            builder.RegisterType<RenewalLicenseStepVm>().Named<ISupervisionStepVm>("20");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("21");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("22");
            builder.RegisterType<OrderStopLicenseStepVm>().Named<ISupervisionStepVm>("23");
            builder.RegisterType<GrantStopLicenseStepVm>().Named<ISupervisionStepVm>("24");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("25");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("26");
            builder.RegisterType<StubWorkingStepVm>().Named<ISupervisionStepVm>("27");
            builder.RegisterType<GrantResultStepVm>().Named<ISupervisionStepVm>("28");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("29");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("30");
            builder.RegisterType<StubWorkingStepVm>().Named<ISupervisionStepVm>("31");
            builder.RegisterType<GrantResultStepVm>().Named<ISupervisionStepVm>("32");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("33");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("34");
            builder.RegisterType<NoticeViolationStepVm>().Named<ISupervisionStepVm>("35");
            builder.RegisterType<OrderAcceptOrRejectStepVm>().Named<ISupervisionStepVm>("36");
            builder.RegisterType<OrderExpertiseStepVm>().Named<ISupervisionStepVm>("37");
            builder.RegisterType<ExpertiseStepVm>().Named<ISupervisionStepVm>("38");
            builder.RegisterType<OrderInspectionStepVm>().Named<ISupervisionStepVm>("39");
            builder.RegisterType<InspectionStepVm>().Named<ISupervisionStepVm>("40");
            builder.RegisterType<OrderGrantLicenseStepVm>().Named<ISupervisionStepVm>("41");
            builder.RegisterType<GrantNewLicenseStepVm>().Named<ISupervisionStepVm>("42");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("43");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("44");
            builder.RegisterType<NoticeViolationStepVm>().Named<ISupervisionStepVm>("45");
            builder.RegisterType<OrderAcceptOrRejectStepVm>().Named<ISupervisionStepVm>("46");
            builder.RegisterType<OrderExpertiseStepVm>().Named<ISupervisionStepVm>("47");
            builder.RegisterType<ExpertiseStepVm>().Named<ISupervisionStepVm>("48");
            builder.RegisterType<OrderInspectionStepVm>().Named<ISupervisionStepVm>("49");
            builder.RegisterType<InspectionStepVm>().Named<ISupervisionStepVm>("50");
            builder.RegisterType<OrderRenewalLicenseStepVm>().Named<ISupervisionStepVm>("51");
            builder.RegisterType<RenewalLicenseStepVm>().Named<ISupervisionStepVm>("52");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("53");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("54");
            builder.RegisterType<OrderStopLicenseStepVm>().Named<ISupervisionStepVm>("55");
            builder.RegisterType<GrantStopLicenseStepVm>().Named<ISupervisionStepVm>("56");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("57");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("58");
            builder.RegisterType<StubWorkingStepVm>().Named<ISupervisionStepVm>("59");
            builder.RegisterType<GrantResultStepVm>().Named<ISupervisionStepVm>("60");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("61");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("62");
            builder.RegisterType<StubWorkingStepVm>().Named<ISupervisionStepVm>("63");
            builder.RegisterType<GrantResultStepVm>().Named<ISupervisionStepVm>("64");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("65");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("66");
            builder.RegisterType<NoticeViolationStepVm>().Named<ISupervisionStepVm>("67");
            builder.RegisterType<OrderAcceptOrRejectStepVm>().Named<ISupervisionStepVm>("68");
            builder.RegisterType<OrderExpertiseStepVm>().Named<ISupervisionStepVm>("69");
            builder.RegisterType<ExpertiseStepVm>().Named<ISupervisionStepVm>("70");
            builder.RegisterType<OrderInspectionStepVm>().Named<ISupervisionStepVm>("71");
            builder.RegisterType<InspectionStepVm>().Named<ISupervisionStepVm>("72");
            builder.RegisterType<OrderGrantLicenseStepVm>().Named<ISupervisionStepVm>("73");
            builder.RegisterType<GrantNewLicenseStepVm>().Named<ISupervisionStepVm>("74");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("75");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("76");
            builder.RegisterType<NoticeViolationStepVm>().Named<ISupervisionStepVm>("77");
            builder.RegisterType<OrderAcceptOrRejectStepVm>().Named<ISupervisionStepVm>("78");
            builder.RegisterType<OrderExpertiseStepVm>().Named<ISupervisionStepVm>("79");
            builder.RegisterType<ExpertiseStepVm>().Named<ISupervisionStepVm>("80");
            builder.RegisterType<OrderInspectionStepVm>().Named<ISupervisionStepVm>("81");
            builder.RegisterType<InspectionStepVm>().Named<ISupervisionStepVm>("82");
            builder.RegisterType<OrderRenewalLicenseStepVm>().Named<ISupervisionStepVm>("83");
            builder.RegisterType<RenewalLicenseStepVm>().Named<ISupervisionStepVm>("84");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("85");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("86");
            builder.RegisterType<OrderStopLicenseStepVm>().Named<ISupervisionStepVm>("87");
            builder.RegisterType<GrantStopLicenseStepVm>().Named<ISupervisionStepVm>("88");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("89");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("90");
            builder.RegisterType<StubWorkingStepVm>().Named<ISupervisionStepVm>("91");
            builder.RegisterType<GrantResultStepVm>().Named<ISupervisionStepVm>("92");

            builder.RegisterType<ResponsibleEmployeeStepVm>().Named<ISupervisionStepVm>("93");
            builder.RegisterType<LinkageStepVm>().Named<ISupervisionStepVm>("94");
            builder.RegisterType<StubWorkingStepVm>().Named<ISupervisionStepVm>("95");
            builder.RegisterType<GrantResultStepVm>().Named<ISupervisionStepVm>("96");

            #endregion
        }
    }
}