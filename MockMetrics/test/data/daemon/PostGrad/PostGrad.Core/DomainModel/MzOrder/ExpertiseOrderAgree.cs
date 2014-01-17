using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.MzOrder
{
    [TableName("gumz.expertise_order_agree")]
    public abstract class ExpertiseOrderAgree : IdentityDomainObject<ExpertiseOrderAgree>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.expertise_order_agree_seq")]
        [MapField("expertise_order_agree_id")]
        public abstract override int Id { get; set; }

        [MapField("employee_name")]
        public abstract string EmployeeName { get; set; }

        [MapField("employee_position")]
        public abstract string EmployeePosition { get; set; }

        [MapField("file_scenario_step_id")]
        public abstract int ExpertiseOrderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ExpertiseOrderId", OtherKey = "Id", CanBeNull = false)]
        public abstract ExpertiseOrder ExpertiseOrder { get; set; }
    }
}