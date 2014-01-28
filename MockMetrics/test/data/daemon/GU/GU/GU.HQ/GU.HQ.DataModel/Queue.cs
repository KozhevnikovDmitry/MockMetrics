using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    [TableName("gu_hq.queue")]
    public abstract class Queue : IdentityDomainObject<Queue>, IPersistentObject 
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.queue_seq")]
        [MapField("queue_id")]
        public override int Id{ get; set; }

        [MapField("queue_type_id")]
        public abstract int QueueTypeId { get; set; }

        [NoInstance]
        [Association(ThisKey = "QueueTypeId", OtherKey = "Id", CanBeNull = false)]
        public abstract QueueType QueueType { get; set; }

        [MapField("agency_id")]
        public abstract int AgencyId { get; set; }

        [MapField("locked")]
        public abstract int Locked { get; set; }
    }
}