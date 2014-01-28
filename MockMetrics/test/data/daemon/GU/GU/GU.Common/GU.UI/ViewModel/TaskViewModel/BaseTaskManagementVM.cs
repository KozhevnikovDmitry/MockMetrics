using System;
using System.Windows;

using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;
using GU.BL;
using GU.DataModel;
using GU.UI.View;
using GU.UI.View.ReportDialogView;
using GU.UI.View.TaskView;
using GU.UI.ViewModel.ReportDialogViewModel;

using Microsoft.Practices.Prism.Commands;

namespace GU.UI.ViewModel.TaskViewModel
{
    /// <summary>
    /// Класс VM для View модуля работы с заявлениями.
    /// </summary>
    public class BaseTaskManagementVM : BaseAvalonDockVM
    {
        /// <summary>
        /// Класс VM для View модуля лицензирования.
        /// </summary>
        /// <param name="isDebug">Флажок указывающий на дебажность запущенной сборки.</param>
        public BaseTaskManagementVM(bool isDebug)
            : this(new SingletonDockableUiFactory())
        {
            IsDebug = isDebug;
        }

        /// <summary>
        /// Класс VM для View модуля лицензирования.
        /// </summary>
        protected BaseTaskManagementVM(IDockableUiFactory dockableUiFactory)
            : base(dockableUiFactory)
        {
            this.ShowTasksCommand = new DelegateCommand(this.ShowTasks);
            this.NewTaskCommand = new DelegateCommand(this.NewTask);
            this.ShowTaskByServiceReportCommand = new DelegateCommand(this.ShowTaskByServiceReport);
            this.ShowTaskByStatusReportCommand = new DelegateCommand(this.ShowTaskByStatusReport);
            this.ShowTaskRegistrReportCommand = new DelegateCommand(this.ShowTaskRegistrReport);
            ShowTaskDataReportCommand = new DelegateCommand(ShowTaskDataReport, CanShowTaskDataReport);

            // свойствам на которые вяжутся имена итемов из тулбара по смене статуса
            // проставляем значения из DictionaryManager'а
            var dict = GuFacade.GetDictionaryManager().GetEnumDictionary<TaskStatusType>();
            TaskStatusNotFilledName = dict[(int)TaskStatusType.NotFilled];
            TaskStatusCheckupWaitingName = dict[(int)TaskStatusType.CheckupWaiting];
            TaskStatusAcceptedName = dict[(int)TaskStatusType.Accepted];
            TaskStatusWorkingName = dict[(int)TaskStatusType.Working];
            TaskStatusReadyName = dict[(int)TaskStatusType.Ready];
            TaskStatusDoneName = dict[(int)TaskStatusType.Done];
            TaskStatusRejectedName = dict[(int)TaskStatusType.Rejected];
        }

        /// <summary>
        /// Флажок указывающий на дебажность запущенной сборки.
        /// </summary>
        public bool IsDebug { get; set; }

        #region Binding Properties

        public bool IsActiveStatement
        {
            get
            {
                return this.IsActiveVMEditableOfDomainType<Task>();
            }
        }

        public string TaskStatusNotFilledName { get; set; }

        public string TaskStatusCheckupWaitingName { get; set; }

        public string TaskStatusAcceptedName { get; set; }

        public string TaskStatusWorkingName { get; set; }

        public string TaskStatusReadyName { get; set; }

        public string TaskStatusDoneName { get; set; }

        public string TaskStatusRejectedName { get; set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда открытия окна поиска заявлений.
        /// </summary>
        public DelegateCommand ShowTasksCommand { get; protected set; }

        /// <summary>
        /// Команда открытия бланка нового заявления.
        /// </summary>
        public DelegateCommand NewTaskCommand { get; protected set; }

        /// <summary>
        /// Открывает бланк нового заявления.
        /// </summary>
        private void NewTask()
        {
            try
            {
                var newTaskVm = new NewTaskVM();
                if (UIFacade.ShowDialogView(new NewTaskView(), newTaskVm, "Новая заявка"))
                {
                    var newTask = GuFacade.GetTaskPolicy().CreateDefaultTask(newTaskVm.Service, newTaskVm.Agency);
                    this.AvalonInteractor.RaiseOpenEditableDockable("Новая заявка", typeof(Task), newTask);
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

        /// <summary>
        /// Открывает окно поиска заявлений.
        /// </summary>
        private void ShowTasks()
        {
            try
            {
                this.AvalonInteractor.RaiseOpenSearchDockable("Заявки", typeof(Task));
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

        #region Gu report

        public DelegateCommand ShowTaskByServiceReportCommand { get; protected set; }

        public DelegateCommand ShowTaskByStatusReportCommand { get; protected set; }

        public DelegateCommand ShowTaskRegistrReportCommand { get; protected set; }

        private void ShowTaskByServiceReport()
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
                    report.Groups = new[] { "ServiceGroup", "Service" };

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                    
                    AddDockable("Отчёт по заявлениям в разрезе услуг", reportView);
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

        private void ShowTaskByStatusReport()
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
                    report.Groups = new[] { "CurrentState" };

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);

                    AddDockable("Отчёт по заявлениям в разрезе статусов", reportView);
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

        private void ShowTaskRegistrReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var date1 = vm.Date1.HasValue ? vm.Date1.Value : DateTime.MinValue;
                    var date2 = vm.Date2.HasValue ? vm.Date2.Value : DateTime.MaxValue;
                    var report = GuFacade.GetTaskRegistrReport(GuFacade.GetDbUser().UserText, date1, date2);

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                    AddDockable("Отчёт по реестру заявлений за период", reportView);
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


        public DelegateCommand ShowTaskDataReportCommand { get; protected set; }

        private void ShowTaskDataReport()
        {
            try
            {
                var task = (this.ActiveWorkspaceVM as TaskDockableVM).Entity;
                var report = GuFacade.GetTaskDataReport(task, GuFacade.GetDbUser().UserText);
                var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                AddDockable("Данные заявки", reportView);
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

        private bool CanShowTaskDataReport()
        {
            return IsActiveVMEditableOfDomainType<Task>();
        }

        #endregion

        #region SetState

        /// <summary>
        /// Команда установки статуса "Незаполнено".
        /// </summary>
        private DelegateCommand _setNotFilledStatusCommand;

        /// <summary>
        /// Команда установки статуса "Ожидает проверки".
        /// </summary>
        private DelegateCommand _setCheckupWaitingStatusCommand;

        /// <summary>
        /// Команда установки статуса "Принято к рассмотрению".
        /// </summary>
        private DelegateCommand _setAcceptedStatusCommand;

        /// <summary>
        /// Команда установки статуса "В работе".
        /// </summary>
        private DelegateCommand _setWorkingStatusCommand;

        /// <summary>
        /// Команда установки статуса "Готово".
        /// </summary>
        private DelegateCommand _setReadyStatusCommand;

        /// <summary>
        /// Команда установки статуса "Услуга предоставлена".
        /// </summary>
        private DelegateCommand _setDoneStatusCommand;

        /// <summary>
        /// Команда установки статуса "Отклонено".
        /// </summary>
        private DelegateCommand _setRejectedStatusCommand;

        /// <summary>
        /// Возвращает команду установки статуса "Незаполнено".
        /// </summary>
        public DelegateCommand SetNotFilledStatusCommand
        {
            get
            {
                if (_setNotFilledStatusCommand == null)
                {
                    _setNotFilledStatusCommand = new DelegateCommand(() => SetState(TaskStatusType.NotFilled), () => IsValidStatusTransition(TaskStatusType.NotFilled));
                }
                return _setNotFilledStatusCommand;
            }
        }

        /// <summary>
        /// Возвращает команду установки статуса "Ожидает проверки".
        /// </summary>
        public DelegateCommand SetCheckupWaitingStatusCommand
        {
            get
            {
                if (_setCheckupWaitingStatusCommand == null)
                {
                    _setCheckupWaitingStatusCommand = new DelegateCommand(() => SetState(TaskStatusType.CheckupWaiting), () => IsValidStatusTransition(TaskStatusType.CheckupWaiting));
                }
                return _setCheckupWaitingStatusCommand;
            }
        }

        /// <summary>
        /// Возвращает команду установки статуса "Принято к рассмотрению".
        /// </summary>
        public DelegateCommand SetAcceptedStatusCommand
        {
            get
            {
                if (_setAcceptedStatusCommand == null)
                {
                    _setAcceptedStatusCommand = new DelegateCommand(() => SetState(TaskStatusType.Accepted), () => IsValidStatusTransition(TaskStatusType.Accepted));
                }
                return _setAcceptedStatusCommand;
            }
        }

        /// <summary>
        /// Возвращает команду установки статуса "В работе".
        /// </summary>
        public DelegateCommand SetWorkingStatusCommand
        {
            get
            {
                if (_setWorkingStatusCommand == null)
                {
                    _setWorkingStatusCommand = new DelegateCommand(() => SetState(TaskStatusType.Working), () => IsValidStatusTransition(TaskStatusType.Working));
                }
                return _setWorkingStatusCommand;
            }
        }

        /// <summary>
        /// Возвращает команду установки статуса "Готово".
        /// </summary>
        public DelegateCommand SetReadyStatusCommand
        {
            get
            {
                if (_setReadyStatusCommand == null)
                {
                    _setReadyStatusCommand = new DelegateCommand(() => SetState(TaskStatusType.Ready), () => IsValidStatusTransition(TaskStatusType.Ready));
                }
                return _setReadyStatusCommand;
            }
        }

        /// <summary>
        /// Возвращает команду установки статуса "Услуга предоставлена".
        /// </summary>
        public DelegateCommand SetDoneStatusCommand
        {
            get
            {
                if (_setDoneStatusCommand == null)
                {
                    _setDoneStatusCommand = new DelegateCommand(() => SetState(TaskStatusType.Done), () => IsValidStatusTransition(TaskStatusType.Done));
                }
                return _setDoneStatusCommand;
            }
        }

        /// <summary>
        /// Возвращает команду установки статуса "Отклонено".
        /// </summary>
        public DelegateCommand SetRejectedStatusCommand
        {
            get
            {
                if (_setRejectedStatusCommand == null)
                {
                    _setRejectedStatusCommand = new DelegateCommand(() => SetState(TaskStatusType.Rejected), () => IsValidStatusTransition(TaskStatusType.Rejected));
                }
                return _setRejectedStatusCommand;
            }
        }

        /// <summary>
        /// Устанавливет статус текущей заявке
        /// </summary>
        /// <param name="taskStatusType">Тип статуса</param>
        private void SetState(TaskStatusType taskStatusType)
        {
            try
            {
                var taskVm = ActiveWorkspaceVM as TaskDockableVM;
                var task = taskVm.Entity;

                var tmp = task.Clone();
                tmp.CurrentState = taskStatusType;

                var validationResult = taskVm.TaskValidator.Validate(tmp);
                taskVm.ContentNodeVM.AllowSinglePropertyValidate = true;
                if (!validationResult.IsValid)
                {
                    UIFacade.ShowToolView(new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
                    taskVm.TaskVM.RaiseIsValidChanged();
                    taskVm.ContentNodeVM.RaiseIsValidChanged();
                    return;
                }

                var addNoteVM = new AddNoteVM();
                string statusName = GuFacade.GetDictionaryManager().GetEnumDictionary<TaskStatusType>()[(int)taskStatusType];
                if (UIFacade.ShowDialogView(new AddNoteView(), addNoteVM, string.Format("Добавить статус {0}", statusName)))
                {
                    if (!this.IsSaveTaskPermited(task))
                    {
                        return;
                    }

                    GuFacade.GetTaskPolicy().SetStatus(taskStatusType, addNoteVM.Comment, task);
                    taskVm.SaveCommand.Execute();
                    RaiseCanSetStateExecuteChanged();
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }

        private bool IsSaveTaskPermited(Task task)
        {
            var mbr = NoticeUser.ShowQuestionYesNo(string.Format("Сохранить изменения в {0}?", task.ToString()));
            return mbr == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Вовзвращает флаг возможности установки статуса текущей заявке
        /// </summary>
        /// <param name="taskStatusType">Тип статуса</param>
        /// <returns>Флаг возможности установки статуса текущей заявке</returns>
        private bool IsValidStatusTransition(TaskStatusType taskStatusType)
        {
            if (ActiveWorkspaceVM is TaskDockableVM)
            {
                var task = (ActiveWorkspaceVM as TaskDockableVM).Entity;
                return GuFacade.GetTaskPolicy().IsValidStatusTransition(task.CurrentState, taskStatusType);
            }

            return false;
        }
        
        /// <summary>
        /// Оповещает команды установки статусов о необходимости сменить режима доступности.
        /// </summary>
        protected virtual void RaiseCanSetStateExecuteChanged()
        {
            SetNotFilledStatusCommand.RaiseCanExecuteChanged();
            SetCheckupWaitingStatusCommand.RaiseCanExecuteChanged();
            SetAcceptedStatusCommand.RaiseCanExecuteChanged();
            SetWorkingStatusCommand.RaiseCanExecuteChanged();
            SetReadyStatusCommand.RaiseCanExecuteChanged();
            SetDoneStatusCommand.RaiseCanExecuteChanged();
            SetRejectedStatusCommand.RaiseCanExecuteChanged();
        }

        #endregion
        
        #endregion

        #region BaseAvalonDockVM

        /// <summary>
        /// Оповещает View об смене активной закладки AvalonDock.
        /// </summary>
        protected override void NotifyActiveDocumentChanged()
        {
            RaiseCanSetStateExecuteChanged();
            this.RaisePropertyChanged(() => IsActiveStatement);
            base.NotifyActiveDocumentChanged();
        }

        #endregion
    }
}
