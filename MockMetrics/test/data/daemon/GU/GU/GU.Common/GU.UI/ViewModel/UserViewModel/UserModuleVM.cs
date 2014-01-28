using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using GU.UI.View.UserView;
using Microsoft.Practices.Prism.Commands;

namespace GU.UI.ViewModel.UserViewModel
{
    public class UserModuleVM : BaseAvalonDockVM
    {
        public UserModuleVM()
            :base(new SingletonDockableUiFactory())
        {
            NewUserCommand = new DelegateCommand(NewUser);
            UserSearchCommand = new DelegateCommand(UserSearch);
        }

        #region Binding Properties

        #endregion

        #region Binding Commands

        /// <summary>
        /// new dbuser command
        /// </summary>
        public DelegateCommand NewUserCommand { get; private set; }

        /// <summary>
        /// new dbuser action
        /// </summary>
        private void NewUser()
        {
            try
            {
                var user = DbUser.CreateInstance();
                AvalonInteractor.RaiseOpenEditableDockable("Новый пользователь", typeof(DbUser), user);
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

        /// <summary>
        /// dbuser search command
        /// </summary>
        public DelegateCommand UserSearchCommand { get; private set; }

        /// <summary>
        /// dbuser search action
        /// </summary>
        private void UserSearch()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Поиск пользователей", typeof(DbUser));
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

        #endregion

    }
}
