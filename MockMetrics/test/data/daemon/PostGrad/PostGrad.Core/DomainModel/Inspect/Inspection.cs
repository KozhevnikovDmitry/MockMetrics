using System;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Inspect
{
    /// <summary>
    /// Класс представляющий сущность Выездная проверка
    /// </summary>
    [TableName("gumz.inspection")]
    public abstract class Inspection : IdentityDomainObject<Inspection>, IPersistentObject
    {
        /// <summary>
        /// Класс представляющий сущность Выездная проверка
        /// </summary>
        public Inspection()
        {
            InspectionEmployeeList = new EditableList<InspectionEmployee>();
            InspectionExpertList = new EditableList<InspectionExpert>();
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
        /// Примечание к выездной проверке
        /// </summary>
        [MapField("inspection_note")]
        public abstract string InspectionNote { get; set; }

        /// <summary>
        /// Список сотрудников привлечённых к выездной проверке
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "InspectionId", CanBeNull = true)]
        public abstract EditableList<InspectionEmployee> InspectionEmployeeList { get; set; }

        /// <summary>
        /// Список экспертов привлечённых к выездной проверке
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "InspectionId", CanBeNull = true)]
        public abstract EditableList<InspectionExpert> InspectionExpertList { get; set; }
    }
}