using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.u_role_child")]
    public abstract class DbRoleChild : DomainObject<DbRoleChild>, IPersistentObject
    {
        ///<summary>
        /// role_id
        ///</summary>
        [MapField("role_id")]
        public abstract int RoleId { get; set; }

        ///<summary>
        /// child_role_id
        ///</summary>
        [MapField("child_role_id")]
        public abstract int ChildRoleId { get; set; }
    }
}
