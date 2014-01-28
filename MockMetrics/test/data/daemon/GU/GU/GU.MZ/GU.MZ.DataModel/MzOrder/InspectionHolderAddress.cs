using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.MzOrder
{
    [TableName("gumz.inspection_holder_address")]
    public abstract class InspectionHolderAddress : IdentityDomainObject<InspectionHolderAddress>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.inspection_holder_address_seq")]
        [MapField("inspection_holder_address_id")]
        public abstract override int Id { get; set; }

        [MapField("address")]
        public abstract string Address { get; set; }

        [MapField("file_scenario_step_id")]
        public abstract int InspectionOrderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "InspectionOrderId", OtherKey = "Id", CanBeNull = false)]
        public abstract InspectionOrder InspectionOrder { get; set; }
    }
}