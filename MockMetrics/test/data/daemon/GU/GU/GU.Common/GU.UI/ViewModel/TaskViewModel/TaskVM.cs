using System.Linq;

using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.ValidationViewModel;

using GU.BL;
using GU.DataModel;

namespace GU.UI.ViewModel.TaskViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных заявки и её истории статусов.
    /// </summary>
    public class TaskVM : ValidateableVM, IAvalonDockCaller
    {
        /// <summary>
        /// Отображаемая заявка
        /// </summary>
        private Task _task;

        /// <summary> 
        /// Класс ViewModel для отображения данных заявки и её истории статусов.
        /// </summary>
        /// <param name="task">Отображаемая заявка</param>
        /// <param name="isEditable">Флаг возможности редактирования</param>
        public TaskVM(Task task, bool isEditable = true)
        {            
            _task = task;
            CreateChildViewModels(isEditable);
            SetTitle(_task.ServiceId);
        }

        /// <summary>
        /// Собирает дочерние VM'ы
        /// </summary>
        /// <param name="isEditable">Флаг возможности редактирования</param>
        private void CreateChildViewModels(bool isEditable)
        {
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            TaskDataVM = (TaskDataVM)UIFacade.GetDomainValidateableVM<Task>(_task);
            TaskDataVM.IsEditable = isEditable;
            this.AvalonInteractor.RegisterCaller(TaskDataVM);
            TaskStatusesVM = new TaskStatusesVM(_task);
        }

        #region Binding Properties

        /// <summary>
        /// VM для отображения и валидации данных самой заявки.
        /// </summary>
        private TaskDataVM _taskDataVM;

        /// <summary>
        /// Возвращает или устанавливает VM для отображения и валидации данных самой заявки.
        /// </summary>
        public TaskDataVM TaskDataVM
        {
            get
            {
                return _taskDataVM;
            }
            set
            {
                if (_taskDataVM != value)
                {
                    _taskDataVM = value;
                    RaisePropertyChanged(() => TaskDataVM);
                }
            }
        }

        /// <summary>
        /// VM для отображения истории статусов заявки
        /// </summary>
        private TaskStatusesVM _taskStatusesVM;

        /// <summary>
        /// Возвращает или устанавливает VM для отображения истории статусов заявки
        /// </summary>
        public TaskStatusesVM TaskStatusesVM
        {
            get
            {
                return this._taskStatusesVM;
            }
            set
            {
                if (this._taskStatusesVM != value)
                {
                    this._taskStatusesVM = value;
                    this.RaisePropertyChanged(() => this.TaskStatusesVM);
                }
            }
        }

        /// <summary>
        /// Отображаемое имя заявки.
        /// </summary>
        private string _taskTitle;

        /// <summary>
        /// Возвращает или устанавливает отображаемое имя заявки.
        /// </summary>
        public string TaskTitle
        {
            get
            {
                return _taskTitle;
            }
            set
            {
                if (_taskTitle != value)
                {
                    _taskTitle = value;
                    RaisePropertyChanged(() => TaskTitle);
                }
            }
        }

        /// <summary>
        /// Строка с информацией о дате последних изменений.
        /// </summary>
        public string LastChangesStamp
        {
            get
            {
                var cd = _task.CommonData;
                if (cd == null)
                {
                    return string.Empty;
                }

                return string.Format("Последнее изменение: {0}", cd.Stamp.ToString());
            }
        }

        /// <summary>
        /// Формирует отображаемое имя заявки.
        /// </summary>
        /// <param name="serviceId">Код услуги, на которую была подана заявка</param>
        private void SetTitle(int serviceId)
        {
            if (serviceId != 0)
            {
                var service = GuFacade.GetDictionaryManager().GetDictionary<Service>().Single(t => t.Id == serviceId);
                TaskTitle = string.Format("Заявка на {0} ({1})", service.Name, service.ServiceGroup.ServiceGroupName);
            }
            else
            {
                TaskTitle = string.Empty;
            }
        }

        #endregion

        #region ValidateableVM 

        /// <summary>
        /// Оповещает представление об обновлении данных по валидируемым полям <c>TaskDataVM</c>.
        /// </summary>
        public override void RaiseValidatingPropertyChanged()
        {
            TaskDataVM.RaiseValidatingPropertyChanged();
        }

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }
}
