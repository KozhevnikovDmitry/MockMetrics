using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Validation;
using Common.DA;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.WeakEvent.EventSubscriber;
using GU.BL;
using GU.BL.DataMapping;
using GU.BL.DomainValidation;
using GU.BL.Policy;
using GU.DataModel;
using GU.UI.View.TaskView;
using GU.UI.View.UserView;
using GU.UI.ViewModel.TaskViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.UI.ViewModel.UserViewModel
{
    public class UserVM : EditableVM<DbUser>
    {
        private readonly IDomainValidator<DbUser> _validator;

        public UserVM(DbUser entity,
                      IDomainObjectEventSubscriber<DbUser> eventSubscriber,
                      IDomainDataMapper<DbUser> dataMapper,
                      IDomainValidator<DbUser> validator,
                      bool isEditable = false)
            : base(entity, eventSubscriber, dataMapper, isEditable)
        {
            _validator = validator;
            AddRoleCommand = new DelegateCommand(AddRole, IsAddRoleAvailable);
        }

        protected override void Rebuild()
        {
            UserDataVM = (UserDataVM) UIFacade.GetDomainValidateableVM(Entity);
            UserManageVM = new UserManageVM(Entity);
            UserManageVM.IsRebuildRequired += (user) =>
                                                  {
                                                      Entity = user;
                                                      Rebuild();
                                                      RaiseDisplayNameChanged(Entity.ToString());
                                                  };
            UserRoleListVM = new UserRoleListVM(Entity);
        }

        protected override void Save()
        {
            try
            {
                var validationResult = _validator.Validate(Entity);

                if (Entity.PersistentState == PersistentState.New && UserManageVM.Password == null)
                {
                    validationResult.AddError("Не установлен пароль");
                }

                if (!validationResult.IsValid)
                {
                    UIFacade.ShowToolView(new ValidationsView(),
                                          new ValidationsVM(validationResult.Errors),
                                          "Ошибочно заполненные поля");
                    UserDataVM.RaiseIsValidChanged();
                    return;
                }
                
                // TODO: нужно еще где-то проверять, что такого пользователя с таким именем нет в системе
                //          либо выбрасывать более няшное сообщение об ошибке

                // TODO: если пользователь текущий - отдать GuCore
                Entity = UserPolicy.SaveDbUser(Entity, _userManageVM.Password, _dataMapper);
                Rebuild();
                RaiseDisplayNameChanged(Entity.ToString());
                RaiseIsDirtyChanged();
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

        #region Binding Properties

        private UserDataVM _userDataVM;

        public UserDataVM UserDataVM
        {
            get
            {
                return _userDataVM;
            }
            set
            {
                if (_userDataVM != value)
                {
                    _userDataVM = value;
                    RaisePropertyChanged(() => UserDataVM);
                }
            }
        }

        private UserManageVM _userManageVM;

        public UserManageVM UserManageVM
        {
            get
            {
                return _userManageVM;
            }
            set
            {
                if (_userManageVM != value)
                {
                    _userManageVM = value;
                    RaisePropertyChanged(() => UserManageVM);
                }
            }
        }

        private UserRoleListVM _userRoleListVM;

        public UserRoleListVM UserRoleListVM
        {
            get
            {
                return _userRoleListVM;
            }
            set
            {
                if (_userRoleListVM != value)
                {
                    _userRoleListVM = value;
                    RaisePropertyChanged(() => UserRoleListVM);
                }
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand AddRoleCommand { get; private set; }

        private void AddRole()
        {
            try
            {
                var vm = new AddRoleVM(Entity);
                if (UIFacade.ShowConfirmableDialogView(new AddRoleView(), vm, "Добавление роли"))
                {
                    var ur = DbUserRole.CreateInstance();
                    ur.RoleId = vm.Role.Id;
                    ur.Role = GuFacade.GetDictionaryManager().GetDictionaryItem<DbRole>(ur.RoleId);
                    ur.User = Entity;
                    Entity.UserRoleList.Add(ur);
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

        public bool IsAddRoleAvailable()
        {
            return Entity.State != DbUserStateType.Deleted;
        }

        #endregion

    }
}
