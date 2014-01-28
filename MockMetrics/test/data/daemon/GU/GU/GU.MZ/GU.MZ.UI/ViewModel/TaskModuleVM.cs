using Common.DA.Interface;
using Common.UI.Interface;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using GU.MZ.UI.ViewModel.TaskViewModel;
using GU.UI.ViewModel.TaskViewModel;

namespace GU.MZ.UI.ViewModel
{
    /// <summary>
    /// Класс VM для View модуля лицензирования.
    /// </summary>
    public class TaskModuleVm : BaseTaskManagementVM, IEditableHostInfo
    {
        private readonly IEditableFacade _editableFacade;

        /// <summary>
        /// Класс VM для View модуля лицензирования.
        /// </summary>
        public TaskModuleVm(IDockableUiFactory uiFactory,
                            IEditableFacade editableFacade,
                            TaskReportsVm taskReportsVm,
                            TaskStatusingVm taskStatusingVm)
            : base(uiFactory)
        {
            TaskReportsVm = taskReportsVm;
            TaskStatusingVm = taskStatusingVm;
            TaskStatusingVm.Initialize(this);
            AvalonInteractor.RegisterCaller(TaskReportsVm);
            AvalonInteractor.RegisterCaller(TaskStatusingVm);
            _editableFacade = editableFacade;
        }

        #region IEditableHostInfo Implementation

        public IEditableVM<T> GetActiveEditableVm<T>() where T : IDomainObject
        {
            if (ActiveWorkspaceVM is IEditableVM<T>)
            {
                return ActiveWorkspaceVM as IEditableVM<T>;
            }

            return null;
        }

        #endregion

        #region Binding Properties

        public TaskReportsVm TaskReportsVm { get; set; }

        public TaskStatusingVm TaskStatusingVm { get; set; }

        #endregion

        #region BaseAvalonDockVM

        /// <summary>
        /// Оповещает View об смене активной закладки AvalonDock.
        /// </summary>
        protected override void NotifyActiveDocumentChanged()
        {
            base.NotifyActiveDocumentChanged();
            TaskStatusingVm.AcceptTaskCommand.RaiseCanExecuteChanged();
            TaskStatusingVm.RejectTaskCommand.RaiseCanExecuteChanged();
        }

        protected override bool PrepareWorkspaceClosing(IDockableVM workspaceVM)
        {
            bool isClosing = false;
            if (workspaceVM is EditableDockableVM)
            {
                var edvm = workspaceVM as EditableDockableVM;
                isClosing = edvm.EditableDataContext.OnClosing(edvm.DisplayName);
                if (isClosing)
                {
                    if (edvm.EditableDataContext is ISmartEditableVm)
                    {
                        _editableFacade.Close(edvm.EditableDataContext as ISmartEditableVm);
                    }
                }
            }
            else
            {
                isClosing = true;
            }

            return isClosing;
        }

        #endregion

        #region BaseTaskManagementVM

        /// <summary>
        /// Оповещает команды установки статусов о необходимости сменить режима доступности.
        /// </summary>
        protected override void RaiseCanSetStateExecuteChanged()
        {
            base.RaiseCanSetStateExecuteChanged();
            TaskStatusingVm.AcceptTaskCommand.RaiseCanExecuteChanged();
            TaskStatusingVm.RejectTaskCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}

