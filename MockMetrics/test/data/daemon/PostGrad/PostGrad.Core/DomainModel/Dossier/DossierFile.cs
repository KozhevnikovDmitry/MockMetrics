using System;
using System.Collections.Generic;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Holder;
using PostGrad.Core.DomainModel.Licensing;
using PostGrad.Core.DomainModel.Person;

namespace PostGrad.Core.DomainModel.Dossier
{
    /// <summary>
    /// Класс представляющий сущность Том лицензионного дела
    /// </summary>
    [TableName("gumz.dossier_file")]
    public abstract partial class DossierFile : IdentityDomainObject<DossierFile>, IPersistentObject
    {
        protected DossierFile()
        {
            DeleteServiceResults = new List<DossierFileServiceResult>();
        }

        #region Fields

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.dossier_file_seq")]
        [MapField("dossier_file_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Регистрационный номер тома
        /// </summary>
        [MapField("reg_number")]
        public abstract int RegNumber { get; set; }

        /// <summary>
        /// Дата заведения тома
        /// </summary>
        [MapField("create_date")]
        public abstract DateTime CreateDate { get; set; }

        /// <summary>
        /// Текущий статус тома
        /// </summary>
        [MapField("current_status")]
        public abstract DossierFileStatus CurrentStatus { get; set; }

        #endregion


        #region Associations

        /// <summary>
        /// Id сущности Сценария ведения тома
        /// </summary>
        [MapField("scenario_id")]
        public int ScenarioId { get; set; }

        /// <summary>
        /// Сценарий ведения тома
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ScenarioId", OtherKey = "Id", CanBeNull = true)]
        public Scenario Scenario { get; set; }

        /// <summary>
        /// Id текущего этапа сценария
        /// </summary>
        [MapField("current_scenario_step_id")]
        public abstract int CurrentScenarioStepId { get; set; }

        /// <summary>
        /// Список этапов ведения данного тома (пройденные + текущий)
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "DossierFileId", CanBeNull = true)]
        public abstract EditableList<DossierFileScenarioStep> DossierFileStepList { get; set; }

        /// <summary>
        /// Id сущности Лицензионное дело.
        /// </summary>
        [MapField("license_dossier_id")]
        public abstract int? LicenseDossierId { get; set; }

        /// <summary>
        /// Лицензионное дело, к которому принадлежит том.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "LicenseDossierId", OtherKey = "Id", CanBeNull = true)]
        public abstract LicenseDossier LicenseDossier { get; set; }

        /// <summary>
        /// Id сущности Заявка
        /// </summary>
        [MapField("task_id")]
        public int TaskId { get; set; }

        /// <summary>
        /// Заявка
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "TaskId", OtherKey = "Id", CanBeNull = false)]
        public Task Task { get; set; }

        /// <summary>
        /// Id сущности Сотрудник, сотрудник ответственный за ведение тома
        /// </summary>
        [MapField("employee_id")]
        public abstract int EmployeeId { get; set; }

        /// <summary>
        /// Сотрудник, отвественный за ведение тома
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "EmployeeId", OtherKey = "Id", CanBeNull = false)]
        public abstract Employee Employee { get; set; }

        /// <summary>
        /// Опись предоставленных документов
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract DocumentInventory DocumentInventory { get; set; }

        /// <summary>
        /// Id результата предоставления услуги
        /// </summary>
        [MapField("file_service_result_id")]
        public abstract int? DossierFileServiceResultId { get; set; }

        /// <summary>
        /// Результат предоставления услуги в томе
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "DossierFileServiceResultId", OtherKey = "Id", CanBeNull = true)]
        public abstract DossierFileServiceResult DossierFileServiceResult { get; set; }

        /// <summary>
        /// Id сущности Реквизиты лицензиата
        /// </summary>
        [MapField("holder_requisites_id")]
        public abstract int? HolderRequisitesId { get; set; }

        /// <summary>
        /// Реквизиты лицензиата, ассоциированные с делом
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "HolderRequisitesId", OtherKey = "Id", CanBeNull = false)]
        public abstract HolderRequisites HolderRequisites { get; set; }

        [MapField("license_id")]
        public abstract int? LicenseId { get; set; }

        [NoInstance]
        [Association(ThisKey = "LicenseId", OtherKey = "Id", CanBeNull = true)]
        public abstract License License { get; set; }

        #endregion


        #region NoMap Properties

        [MapIgnore]
        [NoInstance]
        public virtual LicensedActivity LicensedActivity { get; set; }

        #endregion


        #region Overrides

        public override string ToString()
        {
            return string.Format("№ {0} от {1}", RegNumber, CreateDate.ToLongDateString());
        }

        #endregion
    }
}
