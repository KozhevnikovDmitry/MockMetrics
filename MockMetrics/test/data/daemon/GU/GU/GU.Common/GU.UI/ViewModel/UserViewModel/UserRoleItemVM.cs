using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.UI.ViewModel.ListViewModel;
using GU.DataModel;
using GU.BL.Extensions;

namespace GU.UI.ViewModel.UserViewModel
{
    public class UserRoleItemVM : AbstractListItemVM<DbUserRole>
    {
        public UserRoleItemVM(DbUserRole entity, bool isValidateable)
            : base(entity, isValidateable)
        {
        }

        #region Overrides of AbstractListItemVM<DbRole>

        protected override void Initialize()
        {
            RoleName = Entity.Role.Name;
            if (Entity.Role.DbRoleName != null)
                RoleName += " (" + Entity.Role.DbRoleName + ")";
            var subRoles = string.Join(", ", Entity.Role.Descendants(x => x.ChildRoles).Select(x => x.Name).ToArray());
            if (subRoles != string.Empty)
                SubRoles = "Включает в себя: " + subRoles;
        }

        protected override bool CanRemoveItem()
        {
            return Entity.User.State != DbUserStateType.Deleted;
        }

        #endregion

        public string RoleName { get; set; }

        public string SubRoles { get; set; }
    }
}
