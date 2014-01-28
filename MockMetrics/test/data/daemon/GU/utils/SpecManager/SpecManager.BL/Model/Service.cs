using System;
using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.service")]
    public class Service : IDomainObject
    {
        [PrimaryKey]
        [MapField("service_id")]
        public int Id { get; set; }

        [MapField("service_name")]
        public string Name { get; set; }

        [MapField("spec_id")]
        public int? SpecId { get; set; }

        [MapField("sort_order")]
        public int Order { get; set; }

        [MapField("service_group_id")]
        public int ServiceGroupId { get; set; }

        [NoInstance]
        [Association(ThisKey = "SpecId", OtherKey = "Id", CanBeNull = false)]
        public Spec Spec { get; set; }

        public override string ToString()
        {
            return string.Format("Услуга Id=[{0}] [{1}]", this.Id, this.Name);
        }

    }
}
