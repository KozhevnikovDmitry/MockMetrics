using System;
using System.Linq;

using Common.BL.DataMapping;
using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.WeakEvent.EventSubscriber;
using GU.BL;
using GU.DataModel;
using GU.UI.ViewModel.ContentViewModel;

namespace GU.UI.ViewModel.TaskViewModel
{
    /// <summary>
    /// Класс ViewModel для редактирования сущности Заявка
    /// </summary>
    public class TaskDockableVM : EditableVM<Task>
    {
        private ContentNodeVmBuilder _contentNodeVmBuilder;

        /// <summary>
        /// Валидатор заявок.
        /// </summary>
        public IDomainValidator<Task> TaskValidator { get; set; }

        /// <summary>
        /// Класс ViewModel для редактирования сущности Заявка
        /// </summary>
        /// <param name="entity">Редактируемая заяка</param>
        /// <param name="eventSubscriber">Подписчик слабых событий заявки</param>
        /// <param name="dataMapper">Маппер заявок</param>
        /// <param name="taskValidator">Валидатор заявок</param>
        /// <param name="isEditable">Флаг возможности редактирования</param>
        public TaskDockableVM(Task entity, 
                              IDomainObjectEventSubscriber<Task> eventSubscriber, 
                              IDomainDataMapper<Task> dataMapper, 
                              IDomainValidator<Task> taskValidator,
                              bool isEditable = true)
            : base(entity, eventSubscriber, dataMapper, isEditable)
        {
            this.TaskValidator = taskValidator;
        }

        /// <summary>
        /// Пересобирает поля привязки.
        /// </summary>
        protected override void Rebuild()
        {
            _isEditable = GuFacade.GetTaskPolicy().IsEditable(Entity);
            this.TaskVM = new TaskVM(this.Entity, this._isEditable);
            this.AvalonInteractor.RegisterCaller(TaskVM);
            if (Entity.Content != null)
            {
                if (Entity.Content.RootContentNodes.Count() > 1)
                {
                    throw new VMException("У контента заявки обнаружено более одного потомка");
                }

                if (_contentNodeVmBuilder == null)
                {
                    _contentNodeVmBuilder = new ContentNodeVmBuilder();
                }

                this.ContentNodeVM = _contentNodeVmBuilder.For(Entity.Content.RootContentNodes.First()).Build(_isEditable);
            }
        }

        #region Binding Properties

        /// <summary>
        /// VM для отображения данных заявки.
        /// </summary>
        private TaskVM _taskVM;

        /// <summary>
        /// Возвращает или устанавливает VM для отображения данных заявки. 
        /// </summary>
        public TaskVM TaskVM
        {
            get
            {
                return this._taskVM;
            }
            set
            {
                if (this._taskVM != value)
                {
                    this._taskVM = value;
                    this.RaisePropertyChanged(() => this.TaskVM);
                }
            }
        }
        
        private ContentNodeVM _contentNodeVm;

        public ContentNodeVM ContentNodeVM
        {
            get
            {
                return _contentNodeVm;
            }
            set
            {
                if (_contentNodeVm != value)
                {
                    _contentNodeVm = value;
                    RaisePropertyChanged(() => ContentNodeVM);
                }
            }
        }

        public bool HasContent
        {
            get
            {
                return this.ContentNodeVM != null;
            }
        }

        #endregion

        #region EditableVM

        /// <summary>
        /// Сохраняет данные заявки в БД.
        /// </summary>
        protected override void Save()
        {
            try
            {
                var validationResult = this.TaskValidator.Validate(this.Entity);
                ContentNodeVM.AllowSinglePropertyValidate = true;
                if (!validationResult.IsValid)
                {
                    UIFacade.ShowToolView(new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
                    TaskVM.RaiseIsValidChanged();
                    ContentNodeVM.RaiseIsValidChanged();
                    return;
                }
                
                base.Save();
                
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
    }
}
