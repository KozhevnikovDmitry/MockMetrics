using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel.Inquiry
{
    [TableName("gu.inquiry_status")]
    public abstract class InquiryStatus : IdentityDomainObject<InquiryStatus>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu.inquiry_status_seq")]
        [MapField("inquiry_status_id")]
        public abstract override int Id { get; set; }

        [MapField("inquiry_id")]
        public abstract int InquiryId { get; set; }

        [NoInstance]
        [Association(ThisKey = "InquiryId", OtherKey = "Id", CanBeNull = false)]
        public Inquiry Inquiry { get; set; }

        [MapField("inquiry_status_type_id")]
        public abstract InquiryStatusType State { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("note")]
        public abstract string Note { get; set; }

        [MapField("user_id")]
        public abstract int UserId { get; set; }

        [NoInstance]
        [Association(ThisKey = "UserId", OtherKey = "Id", CanBeNull = false)]
        public DbUser User { get; set; }
    }
}
