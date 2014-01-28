using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    [TableName("gu_hq.queue_claim")]
    public abstract class QueueClaim : IdentityDomainObject<QueueClaim>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.queue_claim_seq")]
        [MapField("queue_claim_id")]
        public override int Id{ get; set; }

        [MapField("queue_id")]
        public abstract int QueueId { get; set; }

        [NoInstance]
        [Association(ThisKey = "QueueId", OtherKey = "Id", CanBeNull = false)]
        public abstract Queue Queue { get; set; }

        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        [MapField("queue_num")]
        public abstract int QueueNum { get; set; }
    }
}