using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.agency")]
    public abstract class Agency : IDomainObject
    {
        [PrimaryKey]
        [MapField("agency_id")]
        public int Id { get; set; }

        [MapField("agency_name")]
        public string Name { get; set; }

        [MapField("agency_long_name")]
        public string LongName { get; set; }

        [MapField("parent_agency_id")]
        public int? ParentAgencyId { get; set; }

        [Association(ThisKey = "ParentAgencyId", OtherKey = "Id", CanBeNull = false)]
        public Agency ParentAgency { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentAgencyId")]
        public List<Agency> ChildAgencyList { get; set; }

        [Association(ThisKey = "Id", OtherKey = "AgencyId")]
        public List<ServiceGroup> ServiceGroupList { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
