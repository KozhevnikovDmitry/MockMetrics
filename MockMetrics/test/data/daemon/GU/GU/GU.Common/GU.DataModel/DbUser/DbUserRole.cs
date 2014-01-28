using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.u_user_role")]
    public abstract class DbUserRole : IdentityDomainObject<DbUserRole>, IPersistentObject
    {
        ///<summary>
        /// user_role_id
        ///</summary>
        [PrimaryKey, Identity]
        [SequenceName("gu.u_user_role_seq")]
        [MapField("user_role_id")]
        public abstract override int Id { get; set; }

        ///<summary>
        /// user_id
        ///</summary>
        [MapField("user_id")]
        public abstract int UserId { get; set; }

        [Association(ThisKey = "UserId", OtherKey = "Id", CanBeNull = true)]
        public DbUser User { get; set; }

        ///<summary>
        /// role_id
        ///</summary>
        [MapField("role_id")]
        public abstract int RoleId { get; set; }

        [Association(ThisKey = "RoleId", OtherKey = "Id")]
        public DbRole Role { get; set; }
    }
}
