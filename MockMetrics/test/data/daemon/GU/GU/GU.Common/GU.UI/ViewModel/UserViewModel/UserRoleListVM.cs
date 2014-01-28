using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.ListViewModel;
using GU.BL;
using GU.DataModel;
using GU.UI.View.UserView;
using Microsoft.Practices.Prism.Commands;

namespace GU.UI.ViewModel.UserViewModel
{
    public class UserRoleListVM : AbstractListVM<DbUserRole>
    {
        private DbUser _user;

        public UserRoleListVM(DbUser user)
            : base(user.UserRoleList)
        {
            _user = user;
            CanAddItems = false;
            CanRemoveItems = true;
            Title = "Список доступных ролей";
        }

        protected override void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                var dlr = NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить данную запись?");
                if (dlr == MessageBoxResult.Yes)
                {
                    var userRole = (DbUserRole) e.Result;
                    _user.UserRoleList.Remove(userRole);
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
    }
}
