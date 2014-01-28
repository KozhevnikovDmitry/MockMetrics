using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.ValidationViewModel;
using GU.BL;
using GU.BL.Extensions;
using GU.DataModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.UI.ViewModel.UserViewModel
{
    public class AddRoleVM : NotificationObject, IConfirmableVM
    {
        private DbUser _user;

        public AddRoleVM(DbUser user)
        {
            _user = user;
            var roleList = GuFacade.GetDictionaryManager().GetDictionary<DbRole>()
                .Where(x => !x.Descendants(_ => _.ChildRoles, true).Select(z => z.DbRoleName).Contains("gu_user_admin"))
                .OrderBy(x => x.Name).ToList();
            RoleList = new ListCollectionView(roleList);
            if (roleList.Count > 0)
                Role = roleList.First();
        }

        private DbRole _role;
        public DbRole Role
        {
            get
            {
                return _role;
            }
            set
            {
                if (_role != value)
                {
                    _role = value;
                    RaisePropertyChanged(() => Role);
                }
            }
        }

        private ListCollectionView _roleList;
        public ListCollectionView RoleList
        {
            get { return _roleList; }
            set
            {
                if (_roleList != value)
                {
                    _roleList = value;
                    RaisePropertyChanged(() => RoleList);
                }
            }
        }

        #region Implementation of IConfirmableVM

        public void Confirm()
        {
            if(Role==null)
            {
                NoticeUser.ShowWarning("Роль не выбрана");
                return;                
            }

            if (_user.UserRoleList.Count(x => x.RoleId == Role.Id) > 0)
            {
                NoticeUser.ShowWarning("Такая роль уже добавлена");
                return;
            }

            IsConfirmed = true;
        }

        public void ResetAfterFail()
        {
        }

        public bool IsConfirmed { get; private set; }

        #endregion
    }
}
