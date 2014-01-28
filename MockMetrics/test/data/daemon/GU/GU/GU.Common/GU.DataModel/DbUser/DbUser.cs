using System;
using System.Collections.Generic;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using Common.Types;

namespace GU.DataModel
{
    [TableName("gu.u_user")]
    [SearchClass("Пользователь")]
    public abstract class DbUser : IdentityDomainObject<DbUser>, IPersistentObject
    {
        public DbUser()
        {
            UserRoleList = new EditableList<DbUserRole>();
            RoleList = new List<DbRole>();
            PlainRoleList = new List<DbRole>();
            // DefaultValue не работает. Ни над самим енумом, ни над проперти
            State = DbUserStateType.Active;
            AcceptChanges();
        }

        [PrimaryKey, Identity]
        [SequenceName("gu.u_user_seq")]
        [MapField("user_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// логин
        /// </summary>
        [MapField("user_name")]
        [SearchField("Имя", SearchTypeSpec.String)]
        public abstract string Name { get; set; }

        [MapField("status_id")]
        [SearchField("Статус", SearchTypeSpec.Enum)]
        public abstract DbUserStateType State { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [MapField("user_text")]
        [SearchField("ФИО", SearchTypeSpec.String)]
        public abstract string UserText { get; set; }

        /// <summary>
        /// должность
        /// </summary>
        [MapField("appoint_text")]
        [SearchField("Должность", SearchTypeSpec.String)]
        public abstract string AppointText { get; set; }

        [MapField("agency_id")]
        public abstract int? AgencyId { get; set; }

        [Association(ThisKey = "AgencyId", OtherKey = "Id", CanBeNull = false)]
        [SearchField("Ведомство", SearchTypeSpec.Dictionary)]
        public Agency Agency { get; set; }

        ///<summary>
        /// список ролей, навешанных непосредственно на пользователя
        ///</summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "UserId")]
        public abstract EditableList<DbUserRole> UserRoleList { get; set; }

        ///<summary>
        /// список ролей, навешанных непосредственно на пользователя
        ///</summary>
        public List<DbRole> RoleList { get; set; }

        /// <summary>
        /// список всех ролей, включая подчиненные
        /// </summary>
        public List<DbRole> PlainRoleList { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
