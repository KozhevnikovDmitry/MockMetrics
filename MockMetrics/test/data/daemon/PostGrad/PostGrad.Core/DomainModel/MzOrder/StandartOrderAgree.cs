using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.MzOrder
{
    [TableName("gumz.standart_order_agree")]
    public abstract class StandartOrderAgree : IdentityDomainObject<StandartOrderAgree>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.standart_order_agree_seq")]
        [MapField("standart_order_agree_id")]
        public abstract override int Id { get; set; }

        [MapField("employee_name")]
        public abstract string EmployeeName { get; set; }

        [MapField("employee_position")]
        public abstract string EmployeePosition { get; set; }

        [MapField("standart_order_id")]
        public abstract int OrderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "OrderId", OtherKey = "Id", CanBeNull = false)]
        public StandartOrder StandartOrder { get; set; }
    }
}