using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel.Inquiry
{
    [TableName("gu.inquiry_type")]
    public abstract class InquiryType : IdentityDomainObject<InquiryType>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("inquiry_type_id")]
        public abstract override int Id { get; set; }

        [MapField("inquiry_type_name")]
        public abstract string Name { get; set; }

        [MapField("direction_id")]
        public abstract InquiryDirection Direction { get; set; }

        [MapField("request_spec_id")]
        public abstract int RequestSpecId { get; set; }

        [MapField("response_spec_id")]
        public abstract int ResponseSpecId { get; set; }

        [MapField("service_id")]
        public abstract int? ServiceId { get; set; }

        [NoInstance]
        [Association(ThisKey = "RequestSpecId", OtherKey = "Id", CanBeNull = false)]
        public abstract Spec RequestSpec { get; set; }

        [NoInstance]
        [Association(ThisKey = "ResponseSpecId", OtherKey = "Id", CanBeNull = false)]
        public abstract Spec ResponseSpec { get; set; }

        [NoInstance]
        [Association(ThisKey = "ServiceId", OtherKey = "Id", CanBeNull = true)]
        public abstract Service Service { get; set; }

    }
}
