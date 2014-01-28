using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Person;
using GU.MZ.DataModel.Violation;

namespace GU.MZ.DataModel.Dossier
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Этап ведения конкретного тома лицензионного дела
    /// </summary>
    [TableName("gumz.file_scenario_step")] 
    public abstract class DossierFileScenarioStep : IdentityDomainObject<DossierFileScenarioStep>, IPersistentObject
    {
        public DossierFileScenarioStep()
        {
            StandartOrderList = new EditableList<StandartOrder>();
            DeleteNotices = new List<Notice>();
            DeleteViolationNotices = new List<ViolationNotice>();
            DeleteViolationResolveInfos = new List<ViolationResolveInfo>();
            DeleteExpertises = new List<DocumentExpertise>();
            DeleteInspections = new List<Inspection>();
            DeleteExpertiseOrders = new List<ExpertiseOrder>();
            DeleteInspectionOrders = new List<InspectionOrder>();
        }

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.file_scenario_step_seq")]
        [MapField("file_scenario_step_id")]
        public abstract override int Id { get; set; }

        #region Fields

        /// <summary>
        /// Дата начала этапа
        /// </summary>
        [MapField("start_date")]
        public abstract DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания этапа
        /// </summary>
        [MapField("end_date")]
        public abstract DateTime? EndDate { get; set; }

        /// <summary>
        /// Id тома
        /// </summary>
        [MapField("dossier_file_id")]
        public abstract int DossierFileId { get; set; }

        /// <summary>
        /// Том лицензионного дела
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "DossierFileId", OtherKey = "Id", CanBeNull = false)]
        public DossierFile DossierFile { get; set; }

        /// <summary>
        /// Id этапа ведения тома
        /// </summary>
        [MapField("scenario_step_id")]
        public abstract int ScenarioStepId { get; set; }

        /// <summary>
        /// Этап ведения тома
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ScenarioStepId", OtherKey = "Id", CanBeNull = false)]
        public abstract ScenarioStep ScenarioStep { get; set; }

        /// <summary>
        /// Id сотрудник, ответственного за этап ведения тома
        /// </summary>
        [MapField("employee_id")]
        public abstract int EmployeeId { get; set; }

        /// <summary>
        /// Сотрудник, ответственный за этап ведения тома
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "EmployeeId", OtherKey = "Id", CanBeNull = false)]
        public abstract Employee Employee { get; set; }

        #endregion


        #region Child Entities

        /// <summary>
        /// Уведомление о необходимости устранения нарушений
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract ViolationNotice ViolationNotice { get; set; }

        /// <summary>
        /// Уведомление
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract Notice Notice { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract ExpertiseOrder ExpertiseOrder { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract InspectionOrder InspectionOrder { get; set; }

        /// <summary>
        /// Документарная проверка
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract DocumentExpertise DocumentExpertise { get; set; }

        /// <summary>
        /// Выезденая проверка
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract Inspection Inspection { get; set; }

        /// <summary>
        /// Информация об устранении нарушений
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract ViolationResolveInfo ViolationResolveInfo { get; set; }

        /// <summary>
        /// Список типовых приказов
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "FileScenarioStepId", CanBeNull = true)]
        public abstract EditableList<StandartOrder> StandartOrderList { get; set; }

        #endregion


        #region Methods

        public virtual StandartOrder StepStandartOrder()
        {
            if (StandartOrderList == null)
            {
                return null;
            }

            if (StandartOrderList.Count > 1)
            {
                throw new BLLException(string.Format("Найдено более одного приказа на шаге ведения тома лицензионного дела, Id=[{0}]", Id));
            }

            return StandartOrderList.SingleOrDefault();
        }

        #endregion


        #region Delete Cache

        public virtual void AddToDelete(Notice notice)
        {
            if (notice.PersistentState == PersistentState.Old ||
                notice.PersistentState == PersistentState.NewDeleted)
            {
                DeleteNotices.Add(notice);
            }
        }

        public virtual void AddToDelete(ViolationNotice violationNotice)
        {
            if (violationNotice.PersistentState == PersistentState.Old ||
                violationNotice.PersistentState == PersistentState.NewDeleted)
            {
                DeleteViolationNotices.Add(violationNotice);
            }
        }

        public virtual void AddToDelete(ViolationResolveInfo violationResolveInfo)
        {
            if (violationResolveInfo.PersistentState == PersistentState.Old ||
                violationResolveInfo.PersistentState == PersistentState.NewDeleted)
            {
                DeleteViolationResolveInfos.Add(violationResolveInfo);
            }
        }

        public virtual void AddToDelete(Inspection inspection)
        {
            if (inspection.PersistentState == PersistentState.Old ||
                inspection.PersistentState == PersistentState.NewDeleted)
            {
                DeleteInspections.Add(inspection);
            }
        }

        public virtual void AddToDelete(DocumentExpertise documentExpertise)
        {
            if (documentExpertise.PersistentState == PersistentState.Old ||
                documentExpertise.PersistentState == PersistentState.NewDeleted)
            {
                DeleteExpertises.Add(documentExpertise);
            }
        }

        public virtual void AddToDelete(ExpertiseOrder order)
        {
            if (order.PersistentState == PersistentState.Old ||
                order.PersistentState == PersistentState.NewDeleted)
            {
                DeleteExpertiseOrders.Add(order);
            }
        }

        public virtual void AddToDelete(InspectionOrder order)
        {
            if (order.PersistentState == PersistentState.Old ||
                order.PersistentState == PersistentState.NewDeleted)
            {
                DeleteInspectionOrders.Add(order);
            }
        }

        

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<Notice> DeleteNotices { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<ViolationNotice> DeleteViolationNotices { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<Inspection> DeleteInspections { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<DocumentExpertise> DeleteExpertises { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<ExpertiseOrder> DeleteExpertiseOrders { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<InspectionOrder> DeleteInspectionOrders { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<ViolationResolveInfo> DeleteViolationResolveInfos { get; set; }

        #endregion
    }
}
