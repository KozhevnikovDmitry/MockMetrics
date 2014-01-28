using System;
using System.Linq;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.Archive.BL;
using GU.Archive.BL.Reporting.Mapping;
using GU.Archive.DataModel;
using GU.Archive.UI.View;
using GU.BL;
using GU.BL.Reporting.Data;
using GU.BL.Reporting.Mapping;
using GU.DataModel;
using GU.UI.View.ReportDialogView;
using GU.UI.View.TaskView;
using GU.UI.ViewModel.ReportDialogViewModel;
using GU.UI.ViewModel.TaskViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.Archive.UI.ViewModel
{
    public class TaskModuleVM : BaseTaskManagementVM
    {
        public TaskModuleVM()
            :base(new SingletonDockableUiFactory())
        {
            JumpToIncomingCommand = new DelegateCommand(JumpToIncoming, () => IsJumpToIncomingAvailable);
            RegistrateIncomingCommand = new DelegateCommand(RegistrateIncoming, () => IsRegistrateIncomingAvailable);
            ShowTaskByServiceAgencyReportCommand = new DelegateCommand(ShowTaskByServiceAgencyReport);
            ShowTaskByStatusAgencyReportCommand = new DelegateCommand(ShowTaskByStatusAgencyReport);
            ShowTaskByDeliveryTypeReportCommand = new DelegateCommand(ShowTaskByDeliveryTypeReport);
            ShowTaskByRequestTypeReportCommand = new DelegateCommand(ShowTaskByRequestTypeReport);
            ShowTaskByAuthorReportCommand = new DelegateCommand(ShowTaskByAuthorReport);
            ShowTaskByAuthorAgencyReportCommand = new DelegateCommand(ShowTaskByAuthorAgencyReport);
        }

        #region Binding Properties

        #endregion

        #region Binding Commands

        public DelegateCommand JumpToIncomingCommand { get; protected set; }

        private void JumpToIncoming()
        {
            try
            {
                var taskVm = ActiveWorkspaceVM as EditableVM<Task>;
                int taskId = taskVm.Entity.Id;

                using(var db = new ArchiveDbManager())
                {
                    int postId = (from p in db.GetDomainTable<Post>()
                                  where p.TaskId == taskId
                                  select p.Id).FirstOrDefault();

                    if (postId != 0)
                    {
                        var post = ArchiveFacade.GetDataMapper<Post>().Retrieve(postId);
                        AvalonInteractor.RaiseOpenEditableDockable(post.ToString(), typeof(Post), post, true);
                    }
                    else
                        NoticeUser.ShowInformation("Данная заявка не зарегистрирована");
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        public bool IsJumpToIncomingAvailable
        {
            get
            {
                if (!IsActiveVMEditableOfDomainType<Task>())
                    return false;

                var task = (ActiveWorkspaceVM as EditableVM<Task>).Entity;
                if (task.CurrentState == TaskStatusType.None
                    || task.CurrentState == TaskStatusType.NotFilled
                    || task.CurrentState == TaskStatusType.CheckupWaiting)
                {
                    return false;
                }

                return true;
            }
        }

        public DelegateCommand RegistrateIncomingCommand { get; protected set; }

        private void RegistrateIncoming()
        {
            try
            {
                var taskVm = ActiveWorkspaceVM as EditableVM<Task>;

                var validationResult = GuFacade.GetTaskPolicy().ValidateSetStatus(TaskStatusType.Accepted, taskVm.Entity);
                if (validationResult.IsValid)
                {
                    if (taskVm.OnClosing(taskVm.Entity.ToString()) && !taskVm.Entity.IsDirty)
                    {
                        var task = taskVm.Entity.Clone();
                        var regVm = new RegistrateTaskVM(task);
                        if (UIFacade.ShowConfirmableDialogView(new RegistrateTaskView(), regVm, "Регистрация"))
                        {
                            this.ActiveWorkspace.Close();
                            AvalonInteractor.RaiseOpenEditableDockable(regVm.Post.ToString(), typeof(Post), regVm.Post, true);
                        }
                    }
                }
                else
                {
                    UIFacade.ShowToolView(new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public bool IsRegistrateIncomingAvailable
        {
            get
            {
                var taskVM = ActiveWorkspaceVM as EditableVM<Task>;
                if (taskVM != null)
                    if (taskVM.Entity.CurrentState == TaskStatusType.CheckupWaiting)
                        return true;

                return false;
            }
        }

        #endregion

        #region Reports

        public DelegateCommand ShowTaskByServiceAgencyReportCommand { get; protected set; }

        private void ShowTaskByServiceAgencyReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var report = GuFacade.GetTaskStatReport();
                    report.Username = GuFacade.GetDbUser().UserText;
                    report.Header = "Общее количество заявлений в Системе в разрезе услуг";
                    report.Date1 = vm.Date1;
                    report.Date2 = vm.Date2;
                    report.Groups = new[] { "Agency", "ServiceGroup", "Service" };

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);

                    AddDockable("Общее количество заявлений в Системе в разрезе услуг", reportView);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand ShowTaskByStatusAgencyReportCommand { get; protected set; }

        private void ShowTaskByStatusAgencyReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var report = GuFacade.GetTaskStatReport();
                    report.Username = GuFacade.GetDbUser().UserText;
                    report.Header = "Общее количество заявлений в Системе в разрезе статусов заявлений";
                    report.Date1 = vm.Date1;
                    report.Date2 = vm.Date2;
                    report.Groups = new[] { "Agency", "CurrentState" };

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);

                    AddDockable("Общее количество заявлений в Системе в разрезе статусов заявлений", reportView);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand ShowTaskByDeliveryTypeReportCommand { get; protected set; }

        private void ShowTaskByDeliveryTypeReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var report = ArchiveFacade.GetTaskPostStatReport();
                    report.Username = GuFacade.GetDbUser().UserText;
                    report.Header = "Общее количество заявлений в Системе по способу доставки";
                    report.Date1 = vm.Date1;
                    report.Date2 = vm.Date2;
                    report.Groups = new[] { "Agency", "DeliveryType" };

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                    AddDockable("Общее количество заявлений в Системе по способу доставки", reportView);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand ShowTaskByRequestTypeReportCommand { get; protected set; }

        private void ShowTaskByRequestTypeReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var report = ArchiveFacade.GetTaskPostStatReport();
                    report.Username = GuFacade.GetDbUser().UserText;
                    report.Header = "Общее количество заявлений в Системе по типу запроса";
                    report.Date1 = vm.Date1;
                    report.Date2 = vm.Date2;
                    report.Groups = new[] {"Agency", "RequestType"};

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                    AddDockable("Общее количество заявлений в Системе по типу запроса", reportView);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }




        public DelegateCommand ShowTaskByAuthorReportCommand { get; protected set; }
        private void ShowTaskByAuthorReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var report = ArchiveFacade.GetTaskPostStatReport();
                    report.Username = GuFacade.GetDbUser().UserText;
                    report.Header = "Общее количество заявлений в Системе по типу автора";
                    report.Date1 = vm.Date1;
                    report.Date2 = vm.Date2;
                    report.Groups = new[] { "Author" };

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                    AddDockable("Общее количество заявлений в Системе по типу автора", reportView);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand ShowTaskByAuthorAgencyReportCommand { get; protected set; }
        private void ShowTaskByAuthorAgencyReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var report = ArchiveFacade.GetTaskPostStatReport();
                    report.Username = GuFacade.GetDbUser().UserText;
                    report.Header = "Общее количество заявлений в Системе по типу автора и ведомста";
                    report.Date1 = vm.Date1;
                    report.Date2 = vm.Date2;
                    report.Groups = new[] { "Agency", "Author" };

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                    AddDockable("Общее количество заявлений в Системе по типу автора и ведомста", reportView);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion

        protected override void NotifyActiveDocumentChanged()
        {
            RaisePropertyChanged(() => IsJumpToIncomingAvailable);
            JumpToIncomingCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(() => IsRegistrateIncomingAvailable);
            RegistrateIncomingCommand.RaiseCanExecuteChanged();

            base.NotifyActiveDocumentChanged();
        }

        protected override void RaiseCanSetStateExecuteChanged()
        {
            JumpToIncomingCommand.RaiseCanExecuteChanged();
            RegistrateIncomingCommand.RaiseCanExecuteChanged();

            base.RaiseCanSetStateExecuteChanged();
        }

    }
}

