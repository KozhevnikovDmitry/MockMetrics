using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.MzOrder
{
    [TableName("gumz.inspection_order_agree")]
    public abstract class InspectionOrderAgree : IdentityDomainObject<InspectionOrderAgree>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.inspection_order_agree_seq")]
        [MapField("inspection_order_agree_id")]
        public abstract override int Id { get; set; }

        [MapField("employee_name")]
        public abstract string EmployeeName { get; set; }

        [MapField("employee_position")]
        public abstract string EmployeePosition { get; set; }

        [MapField("file_scenario_step_id")]
        public abstract int InspectionOrderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "InspectionOrderId", OtherKey = "Id", CanBeNull = false)]
        public abstract InspectionOrder InspectionOrder { get; set; }
    }
}