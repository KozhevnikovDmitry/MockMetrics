using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.ValidationViewModel;
using GU.BL;
using GU.DataModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.UI.View.TaskView;
using GU.MZ.UI.ViewModel.TaskViewModel;
using GU.UI.View.TaskView;
using GU.UI.ViewModel.TaskViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.HQ.UI.ViewModel
{
    public class HqManagementVM : BaseTaskManagementVM
    {
        public HqManagementVM()
            : base(new SingletonDockableUiFactory())
        {
            AcceptTaskCommand = new DelegateCommand(this.AcceptTask, CanAcceptTask);
            RejectTaskCommand = new DelegateCommand(this.RejectTask, CanRejectTask);
        }

        #region Binding Commands

        /// <summary>
        /// Команда принятия заявления к рассмотрению.
        /// </summary>
        public DelegateCommand AcceptTaskCommand { get; private set; }

        /// <summary>
        /// Принимает заявление к рассмотрению
        /// </summary>
        private void AcceptTask()
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
                        var acceptTaskVm = new AcceptTaskVM(task, HqFacade.GetTaskParser());

                        if (UIFacade.ShowConfirmableDialogView(new AcceptTaskView(), acceptTaskVm, "Принятие к рассмотрению"))
                        {
                            ActiveWorkspace.Close();
                            AvalonInteractor.RaiseOpenEditableDockable(acceptTaskVm.Claim.ToString(), typeof(Claim), acceptTaskVm.Claim, true);
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

        /// <summary>
        /// Возвращает флаг наличия отображаемой заявки, которую можно принять к рассмотрению.
        /// </summary>
        /// <returns></returns>
        private bool CanAcceptTask()
        {
            if (IsActiveVMEditableOfDomainType<Task>())
            {
                var task = (ActiveWorkspaceVM as EditableVM<Task>).Entity;
                var taskPolicy = GuFacade.GetTaskPolicy();
                return taskPolicy.IsValidStatusTransition(task.CurrentState, TaskStatusType.Accepted);
            }
            return false;
        }

        /// <summary>
        /// Команда отклонения заявления.
        /// </summary>
        public DelegateCommand RejectTaskCommand { get; private set; }

        /// <summary>
        /// Отклоняет заявление.
        /// </summary>
        private void RejectTask()
        {
            try
            {   
                var taskVm = ActiveWorkspaceVM as EditableVM<Task>;
                var validationResult = GuFacade.GetTaskPolicy().ValidateSetStatus(TaskStatusType.Rejected, taskVm.Entity);
                if (validationResult.IsValid)
                {
                    var task = taskVm.Entity.Clone();
                    var rejectTaskVm = new RejectTaskVM(task);
                    if (UIFacade.ShowConfirmableDialogView(new RejectTaskView(), rejectTaskVm, "Отклонение"))
                    {
                        Workspaces.Remove(ActiveWorkspace);
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

        /// <summary>
        /// Возвращает флаг наличия отображаемой заявки, которую можно отклонить
        /// </summary>
        /// <returns></returns>
        private bool CanRejectTask()
        {
            if (IsActiveVMEditableOfDomainType<Task>())
            {
                var task = (ActiveWorkspaceVM as EditableVM<Task>).Entity;
                
                // Так как в заявку можно перевети в статус отклонена из люборго статуса то заводим свое ограничение которое позволяет переводить только из статуса ожидает проверки
                return task.CurrentState == TaskStatusType.CheckupWaiting;
            }
            return false;
        }

        #endregion Binding Commands

        #region Domain Objects Open

        /// <summary>
        /// Обрабатывает событие запроса на открытие окна заявления. Открывает окно заявления.
        /// </summary>
        /// <param name="sender">Владелец события</param>
        /// <param name="e">Аргументы события</param>
        private void OnTaskOppeningRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                if (!IsAlreadyOpenned<TaskDockableVM>(e.Result.GetKeyValue()))
                {
                    var ts = GuFacade.GetDataMapper<Task>().Retrieve(e.Result.GetKeyValue());
                    OpenTask(ts, e.Result.ToString());
                }
            }
            catch (BLLException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        /// <summary>
        /// Открывает окно заявления.
        /// </summary>
        /// <param name="task">Объект заявление</param>
        /// <param name="displayName">Отображаемое имя окна</param>
        private void OpenTask(Task task, string displayName)
        {
            try
            {
                AddDockable(displayName, new TaskDockableView { DataContext = UIFacade.GetEditableVM(task) });
            }
            catch (VMException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion Domain Objects Open

        #region BaseAvalonDockVM

        /// <summary>
        /// Оповещает View об смене активной закладки AvalonDock.
        /// </summary>
        protected override void NotifyActiveDocumentChanged()
        {
            base.NotifyActiveDocumentChanged();
            AcceptTaskCommand.RaiseCanExecuteChanged();
            RejectTaskCommand.RaiseCanExecuteChanged();
        }

        #endregion BaseAvalonDockVM

        #region BaseTaskManagementVM

        /// <summary>
        /// Оповещает команды установки статусов о необходимости сменить режима доступности.
        /// </summary>
        protected override void RaiseCanSetStateExecuteChanged()
        {
            base.RaiseCanSetStateExecuteChanged();
            AcceptTaskCommand.RaiseCanExecuteChanged();
            RejectTaskCommand.RaiseCanExecuteChanged();
        }

        #endregion BaseTaskManagementVM
    }
}

