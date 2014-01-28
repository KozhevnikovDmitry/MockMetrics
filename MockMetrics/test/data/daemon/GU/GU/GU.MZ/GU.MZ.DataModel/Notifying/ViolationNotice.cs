using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Notifying
{
    /// <summary>
    /// Уведомление о необходимости устранения нарушений
    /// </summary>
    [TableName("gumz.violation_notice")]
    public abstract class ViolationNotice : IdentityDomainObject<ViolationNotice>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("file_scenario_step_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Лицензиат
        /// </summary>
        [MapField("license_holder")]
        public abstract string LicenseHolder { get; set; }

        /// <summary>
        /// Адрес лицензиата
        /// </summary>
        [MapField("holder_address")]
        public abstract string Address { get; set; }

        /// <summary>
        /// Имя сотрудника принявшего опись
        /// </summary>
        [MapField("employee_name")]
        public abstract string EmployeeName { get; set; }

        /// <summary>
        /// Должность сотрудника принявшего опись
        /// </summary>
        [MapField("employee_position")]
        public abstract string EmployeePosition { get; set; }

        /// <summary>
        /// Наименование лицензируемой деятельности
        /// </summary>
        [MapField("licensed_activity")]
        public abstract string LicensedActivity { get; set; }

        /// <summary>
        /// Дата подачи заявления
        /// </summary>
        [MapField("task_stamp")]
        public abstract DateTime TaskStamp { get; set; }

        /// <summary>
        /// Регистрационный номер заявления
        /// </summary>
        [MapField("task_regnumber")]
        public abstract int TaskRegNumber { get; set; }
        
        /// <summary>
        /// Список нарушений
        /// </summary>
        [MapField("violations")]
        public abstract string Violations { get; set; }

        public override string ToString()
        {
            return string.Format("Уведомление о нарушениях №{0}", Id);
        }
    }
}
