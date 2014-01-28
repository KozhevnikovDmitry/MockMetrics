using Autofac;
using Common.UI;
using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.View.DossierFileView;
using GU.MZ.UI.View.EmployeeView;
using GU.MZ.UI.View.ExpertView;
using GU.MZ.UI.View.LicenseDossierView;
using GU.MZ.UI.View.LicenseHolderView;
using GU.MZ.UI.View.LicenseView;
using GU.MZ.UI.View.OrderView.Expertise;
using GU.MZ.UI.View.OrderView.Inspection;
using GU.MZ.UI.View.OrderView.Standart;
using GU.MZ.UI.ViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using GU.UI.View.TaskView;
using GU.UI.View.UserView;
using GU.UI.ViewModel.TaskViewModel;

namespace GU.MZ.UI.Modules
{
    public class MzUiModule : UiModule
    {
        public MzUiModule()
            : base(new[] { typeof(LicenseModuleVm).Assembly, typeof(TaskVM).Assembly })
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<SupervisionVmModule>();
            builder.RegisterModule<MzSpecialVmModule>();
            builder.RegisterModule(new GenericVmModule(_uiAssemblies));
            RegisterEditableViews(builder);

            builder.RegisterType<EntityFacade>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MzUiFactory>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<SupervisionViewFacotry>().AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
        }

        private void RegisterEditableViews(ContainerBuilder builder)
        {
            RegisterEditableView<TaskDockableView, Task>(builder);
            RegisterEditableView<LicenseDossierView, LicenseDossier>(builder);
            RegisterEditableView<LicenseHolderView, LicenseHolder>(builder);
            RegisterEditableView<DossierFileView, DossierFile>(builder);
            RegisterEditableView<EmployeeView, Employee>(builder);
            RegisterEditableView<LicenseDockableView, License>(builder);
            RegisterEditableView<ExpertView, Expert>(builder);
            RegisterEditableView<UserView, DbUser>(builder);
            RegisterEditableView<InventoryView, DocumentInventory>(builder);
            RegisterEditableView<ExpertiseOrderView, ExpertiseOrder>(builder);
            RegisterEditableView<InspectionOrderView, InspectionOrder>(builder);
            RegisterEditableView<StandartOrderView, StandartOrder>(builder);
        }
    }
}
