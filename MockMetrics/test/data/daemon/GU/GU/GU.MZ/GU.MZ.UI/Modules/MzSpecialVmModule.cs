using Autofac;
using GU.MZ.UI.ViewModel;
using GU.MZ.UI.ViewModel.DossierFileViewModel;
using GU.MZ.UI.ViewModel.EmployeeViewModel;
using GU.MZ.UI.ViewModel.Import;
using GU.MZ.UI.ViewModel.LinkageViewModel;
using GU.MZ.UI.ViewModel.OrderViewModel;
using GU.MZ.UI.ViewModel.OrderViewModel.Inspection;
using GU.MZ.UI.ViewModel.ReportDialogViewModel;
using GU.MZ.UI.ViewModel.SupervisionViewModel;
using GU.MZ.UI.ViewModel.TaskViewModel;

namespace GU.MZ.UI.Modules
{
    public class MzSpecialVmModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainVm>();
            builder.RegisterType<TaskModuleVm>();
            builder.RegisterType<LicenseModuleVm>();

            builder.RegisterType<FileSupervisionVm>();
            builder.RegisterType<DossierFileSourceVm>();
            builder.RegisterType<DossierFileOrdersVm>();
            builder.RegisterType<LinkageDossierVm>();
            builder.RegisterType<LinkageHolderVm>();
            builder.RegisterType<LinkageLicenseVm>();

            builder.RegisterType<AcceptTaskVm>();
            builder.RegisterType<ChooseResponsibleEmployeeVm>();
            builder.RegisterType<RejectTaskVm>();
            builder.RegisterType<NewFullActivityReportVm>();
            builder.RegisterType<ImportVm>();
            builder.RegisterType<AddHolderAddressForOrderVm>();
            builder.RegisterType<AddOrderAgreeVm>();
            builder.RegisterType<AddInspectionOrderExpertVm>();
            builder.RegisterType<TaskStatusingVm>();
            builder.RegisterType<TaskReportsVm>();
        }
    }
}