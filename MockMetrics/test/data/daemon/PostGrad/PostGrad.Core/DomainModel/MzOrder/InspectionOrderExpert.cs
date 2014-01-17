using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.MzOrder
{
    [TableName("gumz.inspection_order_expert")]
    public abstract class InspectionOrderExpert : IdentityDomainObject<InspectionOrderExpert>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.inspection_order_expert_seq")]
        [MapField("inspection_order_expert_id")]
        public abstract override int Id { get; set; }

        [MapField("expert_name")]
        public abstract string ExpertName { get; set; }

        [MapField("expert_position")]
        public abstract string ExpertPosition { get; set; }

        [MapField("file_scenario_step_id")]
        public abstract int InspectionOrderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "InspectionOrderId", OtherKey = "Id", CanBeNull = false)]
        public abstract InspectionOrder InspectionOrder { get; set; }
    }
}