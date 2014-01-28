using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.agency")]
    public abstract class Agency : IdentityDomainObject<Agency>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("agency_id")]
        public abstract override int Id { get; set; }

        [MapField("agency_name")]
        public abstract string Name { get; set; }

        [MapField("agency_long_name")]
        public abstract string LongName { get; set; }

        [MapField("enisey_code")]
        public abstract string EniseyCode { get; set; }

        [MapField("parent_agency_id")]
        public abstract int? ParentAgencyId { get; set; }

        [Association(ThisKey = "ParentAgencyId", OtherKey = "Id", CanBeNull = false)]
        public Agency ParentAgency { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentAgencyId")]
        public List<Agency> ChildAgencyList { get; set; }

        [Association(ThisKey = "Id", OtherKey = "AgencyId")]
        public List<ServiceGroup> ServiceGroupList { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
