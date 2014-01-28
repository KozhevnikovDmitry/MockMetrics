using System;

using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Inspect
{
    /// <summary>
    /// Класс представляющий сущность Документарная экспертиза, проверка соотвествия.
    /// </summary>
    [TableName("gumz.document_expertise")]
    public abstract class DocumentExpertise : IdentityDomainObject<DocumentExpertise>, IPersistentObject
    {
        /// <summary>
        /// Класс представляющий сущность Документарная экспертиза, проверка соотвествия.
        /// </summary>
        public DocumentExpertise()
        {
            this.ExperiseResultList = new EditableList<DocumentExpertiseResult>();
        }

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("file_scenario_step_id")] 
        public abstract override int Id { get; set; }

        /// <summary>
        /// Возвращает флаг успешного прохождения экспертизы полноты
        /// </summary>
        [MapField("is_passed")]
        public abstract bool IsPassed { get; set; }

        /// <summary>
        /// Дата акта проверки
        /// </summary>
        [MapField("act_stamp")]
        public abstract DateTime ActStamp { get; set; }

        /// <summary>
        /// Дата начала проверки
        /// </summary>
        [MapField("start_stamp")]
        public abstract DateTime StartStamp { get; set; }

        /// <summary>
        /// Дата окончания проверки
        /// </summary>
        [MapField("end_stamp")]
        public abstract DateTime EndStamp { get; set; }

        /// <summary>
        /// Список результатов документарной проверки
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "DocumentExpertiseId", CanBeNull = true)]
        public abstract EditableList<DocumentExpertiseResult> ExperiseResultList { get; set; }
    }
}