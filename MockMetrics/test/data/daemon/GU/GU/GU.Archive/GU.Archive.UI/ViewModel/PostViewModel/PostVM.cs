using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel.ValidationViewModel;
using GU.Archive.BL;
using GU.Archive.DataModel;
using Common.UI.ViewModel;
using Common.UI.WeakEvent.EventSubscriber;
using Common.BL.DataMapping;
using GU.Archive.UI.View.PostView;
using GU.UI.View.TaskView;
using GU.UI.ViewModel.TaskViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.Archive.UI.ViewModel.PostViewModel
{
    public class PostVM : EditableVM<Post>
    {
        public PostVM(Post entity,
                      IDomainObjectEventSubscriber<Post> eventSubscriber,
                      IDomainDataMapper<Post> dataMapper,
                      bool isEditable = true)
            : base(entity, eventSubscriber, dataMapper, isEditable)
        {
            AddExecutorCommand = new DelegateCommand(AddExecutor);
        }

        protected override void Rebuild()
        {
            PostDataVM = new PostDataVM(Entity);
            PostExecutorsVM = new PostExecutorsVM(Entity);
        }

        protected override void Save()
        {
            try
            {
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    UIFacade.ShowToolView(new ValidationsView(),
                                          new ValidationsVM(validationResult.Errors),
                                          "Ошибочно заполненные поля");
                    return;
                }

                base.Save();
                RaiseDisplayNameChanged(string.Format("Корреспонденция, №{0}", Entity.RegistrationNum));
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

        private ValidationErrorInfo Validate()
        {
            var validationResult = new ValidationErrorInfo();

            if (Entity.OrganizationId == 0)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Организация"));
            if (Entity.PostType == 0)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Тип корреспонденции"));
            if (Entity.DeliveryType == 0)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Способ доставки"));

            return validationResult;
        }

        #region Binding Properties

        private PostExecutorsVM _postExecutorsVM;

        public PostExecutorsVM PostExecutorsVM
        {
            get
            {
                return _postExecutorsVM;
            }
            set
            {
                if (_postExecutorsVM != value)
                {
                    _postExecutorsVM = value;
                    RaisePropertyChanged(() => PostExecutorsVM);
                }
            }
        }

        private PostDataVM _postDataVM;

        public PostDataVM PostDataVM
        {
            get
            {
                return _postDataVM;
            }
            set
            {
                if (_postDataVM != value)
                {
                    _postDataVM = value;
                    RaisePropertyChanged(() => PostDataVM);
                }
            }
        }

        #endregion

        public DelegateCommand AddExecutorCommand { get; private set; }

        private void AddExecutor()
        {
            try
            {
                var vm = new AddPostExecutorVM();
                if (UIFacade.ShowConfirmableDialogView(new AddPostExecutorView(), vm, "Добавление исполнителя"))
                {
                    var pe = PostExecutor.CreateInstance();
                    pe.PostId = Entity.Id;
                    pe.Stamp = DateTime.Now;
                    pe.EmployeeId = vm.Employee.Id;
                    pe.Employee = vm.Employee;
                    pe.Note = vm.Note;
                    Entity.Executors.Add(pe);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Unexpectable error", ex));
            }
        }
    }
}
