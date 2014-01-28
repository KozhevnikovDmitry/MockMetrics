using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.ValidationViewModel;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.View.TaskView;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{
    public class TaskStatusingVm : NotificationObject, IAvalonDockCaller
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly ITaskPolicy _taskPolicy;
        private readonly AcceptTaskVm _acceptTaskVm;
        private readonly RejectTaskVm _rejectTaskVm;
        private IEditableHostInfo _editableHostInfo;

        public TaskStatusingVm(IDialogUiFactory uiFactory, ITaskPolicy taskPolicy, AcceptTaskVm acceptTaskVm, RejectTaskVm rejectTaskVm)
        {
            _uiFactory = uiFactory;
            _taskPolicy = taskPolicy;
            _acceptTaskVm = acceptTaskVm;
            _rejectTaskVm = rejectTaskVm;
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            AcceptTaskCommand = new DelegateCommand(AcceptTask, CanAcceptTask);
            RejectTaskCommand = new DelegateCommand(RejectTask, CanRejectTask);
        }

        public void Initialize(IEditableHostInfo editableHostInfo)
        {
            _editableHostInfo = editableHostInfo;
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
                var taskVm = _editableHostInfo.GetActiveEditableVm<Task>();
                var validationResult = _taskPolicy.ValidateSetStatus(TaskStatusType.Accepted, taskVm.Entity);
                if (validationResult.IsValid)
                {
                    if (taskVm.OnClosing(taskVm.Entity.ToString()) && !taskVm.Entity.IsDirty)
                    {
                        var task = taskVm.Entity.Clone();
                        _acceptTaskVm.Initialize(task);
                        if (_uiFactory.ShowConfirmableDialogView(
                            new AcceptTaskView(), _acceptTaskVm, "Принятие к рассмотрению"))
                        {
                            AvalonInteractor.RaiseCloseEditableDockable(typeof(Task), task.Id);
                            var dossierFile = _acceptTaskVm.DossierFile;
                            AvalonInteractor.RaiseOpenEditableDockable(
                                dossierFile.ToString(), typeof(DossierFile), dossierFile, true);
                        }
                    }
                }
                else
                {
                    _uiFactory.ShowToolView(
                        new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
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
            var vm = _editableHostInfo.GetActiveEditableVm<Task>();
            if (vm != null)
            {
                var task = vm.Entity;
                return _taskPolicy.IsValidStatusTransition(task.CurrentState, TaskStatusType.Accepted);
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
                var taskVm = _editableHostInfo.GetActiveEditableVm<Task>();
                var validationResult = _taskPolicy.ValidateSetStatus(TaskStatusType.Rejected, taskVm.Entity);
                if (validationResult.IsValid)
                {
                    var task = taskVm.Entity.Clone();
                    _rejectTaskVm.Initialize(task);
                    if (_uiFactory.ShowConfirmableDialogView(new RejectTaskView(), _rejectTaskVm, "Отклонение"))
                    {
                        AvalonInteractor.RaiseCloseEditableDockable(typeof(Task), task.Id);
                    }
                }
                else
                {
                    _uiFactory.ShowToolView(
                        new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
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
            var vm = _editableHostInfo.GetActiveEditableVm<Task>();
            if (vm != null)
            {
                var task = vm.Entity;
                return _taskPolicy.IsValidStatusTransition(task.CurrentState, TaskStatusType.Rejected);
            }
            return false;
        }

        #endregion

        public IAvalonDockInteractor AvalonInteractor { get; private set; }
    }
}