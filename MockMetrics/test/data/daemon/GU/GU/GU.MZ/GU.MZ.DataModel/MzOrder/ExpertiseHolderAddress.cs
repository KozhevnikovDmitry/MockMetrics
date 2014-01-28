using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.MzOrder
{
    [TableName("gumz.expertise_holder_address")]
    public abstract class ExpertiseHolderAddress : IdentityDomainObject<ExpertiseHolderAddress>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.expertise_holder_address_seq")]
        [MapField("expertise_holder_address_id")]
        public abstract override int Id { get; set; }

        [MapField("address")]
        public abstract string Address { get; set; }

        [MapField("file_scenario_step_id")]
        public abstract int ExpertiseOrderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ExpertiseOrderId", OtherKey = "Id", CanBeNull = false)]
        public abstract ExpertiseOrder ExpertiseOrder { get; set; }
    }
}