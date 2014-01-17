using System;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Dossier
{
    [TableName("gumz.doc_inventory")]
    public abstract class DocumentInventory : IdentityDomainObject<DocumentInventory>, IPersistentObject
    {
        public DocumentInventory()
        {
            ProvidedDocumentList = new EditableList<ProvidedDocument>();
        }

        [PrimaryKey]
        [MapField("dossier_file_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Регистрационный номер описи предоставленных документов
        /// </summary>
        [MapField("inventory_reg_number")]
        public abstract int RegNumber { get; set; }

        /// <summary>
        /// Дата составления описи предоставленных документов
        /// </summary>
        [MapField("inventory_stamp")]
        public abstract DateTime Stamp { get; set; }
        
        /// <summary>
        /// Лицензиат
        /// </summary>
        [MapField("license_holder")]
        public abstract string LicenseHolder { get; set; }

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
        /// Список документов приложенных к заявке
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "DossierFileId", CanBeNull = true)]
        public abstract EditableList<ProvidedDocument> ProvidedDocumentList { get; set; }
        
        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public int ServiceId { get; set; }

        public override string ToString()
        {
            return string.Format("Опись №{0} от {1}", RegNumber, Stamp.ToShortDateString());
        }

       
    }
}
