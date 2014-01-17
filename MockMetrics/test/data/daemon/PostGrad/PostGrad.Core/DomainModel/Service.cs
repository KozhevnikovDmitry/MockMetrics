using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel
{
    [TableName("gu.service")]
    public abstract class Service : IdentityDomainObject<Service>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("service_id")]
        public abstract override int Id { get; set; }

        [MapField("service_name")]
        public abstract string Name { get; set; }

        [MapField("spec_id")]
        public abstract int? SpecId { get; set; }

        [MapField("duration_str")]
        public abstract string Duration { get; set; }

        [MapField("max_duration")]
        public abstract int? MaxDuration { get; set; }

        [MapField("tag")]
        public abstract string Tag { get; set; }

        [MapField("namespace")]
        public abstract string Namespace { get; set; }

        [MapField("sort_order")]
        public abstract int Order { get; set; }

        [NoInstance]
        [Association(ThisKey = "ServiceGroupId", OtherKey = "Id", CanBeNull = false)]
        public abstract ServiceGroup ServiceGroup { get; set; }

        [MapField("service_group_id")]
        public abstract int ServiceGroupId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, ServiceGroup.ServiceGroupName);
        }

    }
}
