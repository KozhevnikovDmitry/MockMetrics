using System;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.DataModel.MzOrder
{
    /// <summary>
    /// Приказ о проведении проверки
    /// </summary>
    [TableName("gumz.expertise_order")]
    public abstract class ExpertiseOrder : IdentityDomainObject<ExpertiseOrder>, IPersistentObject, IOrder
    {
        public ExpertiseOrder()
        {
            ExpertiseOrderAgreeList = new EditableList<ExpertiseOrderAgree>();
            ExpertiseHolderAddressList = new EditableList<ExpertiseHolderAddress>();
        }

        [PrimaryKey]
        [MapField("file_scenario_step_id")]
        public abstract override int Id { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("reg_number")]
        public abstract string RegNumber { get; set; }

        [MapField("employee_name")]
        public abstract string EmployeeName { get; set; }

        [MapField("employee_position")]
        public abstract string EmployeePosition { get; set; }

        [MapField("employee_contacts")]
        public abstract string EmployeeContacts { get; set; }

        [MapField("inn")]
        public abstract string Inn { get; set; }

        [MapField("full_name")]
        public abstract string FullName { get; set; }

        [MapField("address")]
        public abstract string Address { get; set; }

        [MapField("licensed_activity")]
        public abstract string LicensedActivity { get; set; }

        [MapField("activity_aditional")]
        public abstract string ActivityAdditionalInfo { get; set; }

        [MapField("task_id")]
        public abstract int TaskId { get; set; }

        [MapField("task_stamp")]
        public abstract DateTime TaskStamp { get; set; }

        [MapField("due_days")]
        public abstract int DueDays { get; set; }

        [MapField("start_date")]
        public abstract DateTime StartDate { get; set; }

        [MapField("due_date")]
        public abstract DateTime DueDate { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ExpertiseOrderId", CanBeNull = true)]
        public abstract EditableList<ExpertiseHolderAddress> ExpertiseHolderAddressList { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ExpertiseOrderId", CanBeNull = true)]
        public abstract EditableList<ExpertiseOrderAgree> ExpertiseOrderAgreeList { get; set; }

        public override string ToString()
        {
            return string.Format("Приказ о проверке №{0} от {1}", RegNumber, Stamp.ToShortDateString());
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public DossierFileScenarioStep FileScenarioStep { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public string Name
        {
            get
            {
                return "Приказ о проведении документарной проверки";
            }
        }
    }
}