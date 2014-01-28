using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

using GU.MZ.DataModel.Person;

namespace GU.MZ.DataModel.Inspect
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Сотрудник, привлечённый к выездной проверке
    /// </summary>
    [TableName("gumz.inspection_employee")]
    public abstract class InspectionEmployee : IdentityDomainObject<InspectionEmployee>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.inspection_employee_seq")] 
        [MapField("inspection_employee_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Id сотрудника
        /// </summary>
        [MapField("employee_id")]
        public abstract int EmployeeId { get; set; }

        /// <summary>
        /// Сотрудник, привлечённый к выездной проверке
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "EmployeeId", OtherKey = "Id", CanBeNull = false)]
        public abstract Employee Employee { get; set; }

        /// <summary>
        /// Id выездной проверки
        /// </summary>
        [MapField("inspection_id")]
        public abstract int InspectionId { get; set; }

        /// <summary>
        /// Выездная проверка
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "InspectionId", OtherKey = "Id", CanBeNull = false)]
        public abstract Inspection Inspection { get; set; }
    }
}
