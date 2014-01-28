using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Common.DA;
using Common.Types.Exceptions;
using Common.UI;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using GU.UI.View.UserView;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.UI.ViewModel.UserViewModel
{
    public class UserManageVM : NotificationObject
    {
        private DbUser _dbUser;

        // SecureString?..
        public string Password { get; private set; }

        public event Action<DbUser> IsRebuildRequired;

        public UserManageVM(DbUser user)
        {
            _dbUser = user;

            ActivateUserCommand = new DelegateCommand(ActivateUser, IsActivateUserAvailable);
            BlockUserCommand = new DelegateCommand(BlockUser, IsBlockUserAvailable);
            DeleteUserCommand = new DelegateCommand(DeleteUser, IsDeleteUserAvailable);
            ChangePasswordCommand = new DelegateCommand(ChangePassword, IsChangePasswordAvailable);
            SetPasswordCommand = new DelegateCommand(SetPassword, IsSetPasswordAvailable);
        }

        #region Binding Properties

        #endregion

        #region Binding Commands

        /// <summary>
        /// активировать пользователя, если у того статус Disabled
        /// </summary>
        public DelegateCommand ActivateUserCommand { get; private set; }

        private void ActivateUser()
        {
            var dlr = NoticeUser.ShowQuestionYesNo("Вы уверены, что хотите разблокировать данного пользователя?");
            if (dlr != MessageBoxResult.Yes)
                return;

            try
            {
                _dbUser = UserPolicy.Activate(_dbUser);
                IsRebuildRequired(_dbUser);
                NoticeUser.ShowInformation("Пользователь успешно разблокирован");
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

        public bool IsActivateUserAvailable()
        {
            return _dbUser.PersistentState == PersistentState.Old && _dbUser.State == DbUserStateType.Disabled;
        }

        /// <summary>
        /// блокировать пользователя, если у того статус Active
        /// </summary>
        public DelegateCommand BlockUserCommand { get; private set; }

        private void BlockUser()
        {
            var dlr = NoticeUser.ShowQuestionYesNo("Вы уверены, что хотите заблокировать данного пользователя?");
            if (dlr != MessageBoxResult.Yes)
                return;

            try
            {
                _dbUser = UserPolicy.Block(_dbUser);
                IsRebuildRequired(_dbUser);
                NoticeUser.ShowInformation("Пользователь успешно заблокирован");
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

        public bool IsBlockUserAvailable()
        {
            return _dbUser.PersistentState == PersistentState.Old && _dbUser.State == DbUserStateType.Active;
        }

        /// <summary>
        /// удалить пользователя
        /// </summary>
        public DelegateCommand DeleteUserCommand { get; private set; }

        private void DeleteUser()
        {
            var dlr = NoticeUser.ShowQuestionYesNo("Вы уверены, что хотите удалить данного пользователя?");
            if (dlr != MessageBoxResult.Yes)
                return;

            try
            {
                _dbUser = UserPolicy.Delete(_dbUser);
                IsRebuildRequired(_dbUser);
                NoticeUser.ShowInformation("Пользователь удален");
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

        public bool IsDeleteUserAvailable()
        {
            return _dbUser.PersistentState == PersistentState.Old && _dbUser.State != DbUserStateType.Deleted;
        }

        /// <summary>
        /// сменить пароль
        /// </summary>
        public DelegateCommand ChangePasswordCommand { get; private set; }

        private void ChangePassword()
        {
            try
            {
                var vm = new SetPasswordVM();
                if (UIFacade.ShowConfirmableDialogView(new SetPasswordView(), vm, "Смена пароля"))
                {
                    UserPolicy.ChangeUserPassword(_dbUser, vm.Password);
                    NoticeUser.ShowInformation("Пароль успешно изменен");
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

        public bool IsChangePasswordAvailable()
        {
            return _dbUser.PersistentState == PersistentState.Old && _dbUser.State != DbUserStateType.Deleted;
        }

        /// <summary>
        /// установить пароль для нового пользователя
        /// </summary>
        public DelegateCommand SetPasswordCommand { get; private set; }

        private void SetPassword()
        {
            try
            {
                var vm = new SetPasswordVM();
                if (UIFacade.ShowConfirmableDialogView(new SetPasswordView(), vm, "Установка пароля"))
                {
                    Password = vm.Password;
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

        public bool IsSetPasswordAvailable()
        {
            return _dbUser.PersistentState == PersistentState.New;
        }

        #endregion

    }
}
