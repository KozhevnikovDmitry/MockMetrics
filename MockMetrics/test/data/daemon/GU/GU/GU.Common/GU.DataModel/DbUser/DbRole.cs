using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.u_role")]
    public abstract class DbRole : IdentityDomainObject<DbRole>, IPersistentObject
    {
        ///<summary>
        /// role_id
        ///</summary>
        [PrimaryKey]
        [MapField("role_id")]
        public abstract override int Id { get; set; }

        ///<summary>
        /// role_name
        ///</summary>
        [MapField("role_name")]
        public abstract string Name { get; set; }

        ///<summary>
        /// db_role_name
        ///</summary>
        [MapField("db_role_name")]
        public abstract string DbRoleName { get; set; }

        ///<summary>
        /// agency_id
        ///</summary>
        [MapField("agency_id")]
        public abstract int? AgencyId { get; set; }

        ///<summary>
        /// Agency
        ///</summary>
        [Association(ThisKey = "AgencyId", OtherKey = "Id", CanBeNull = true)]
        public Agency Agency { get; set; }

        ///<summary>
        /// child role list
        ///</summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = true)]
        public abstract EditableList<DbRole> ChildRoles { get; set; }
    }
}
