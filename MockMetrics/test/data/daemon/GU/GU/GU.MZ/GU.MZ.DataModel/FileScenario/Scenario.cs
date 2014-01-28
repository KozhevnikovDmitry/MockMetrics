using System.Collections.Generic;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

using GU.DataModel;

namespace GU.MZ.DataModel.FileScenario
{
    /// <summary>
    /// Класс, представляющий сущность Сценарий ведения тома лицензионного дела.
    /// </summary>
    [TableName("gumz.scenario")]
    public abstract class Scenario : IdentityDomainObject<Scenario>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.scenario_seq")]
        [MapField("scenario_id")]
        public abstract override int Id { get; set; }
        
        /// <summary>
        /// Флаг, указывающий на то, может ли том ведомый по этому сценарию быть первым в лицензионном деле
        /// </summary>      
        [MapField("can_be_initial")]
        public abstract bool CanBeInitial { get; set; }

        /// <summary>
        /// Тип сценария ведения - полный, облегчённый
        /// </summary>
        [MapField("scenario_type")]
        public abstract ScenarioType ScenarioType { get; set; }
        
        /// <summary>
        /// Id сущности Государственная услуга
        /// </summary>
        [MapField("service_id")]
        public abstract int ServiceId { get; set; }

        /// <summary>
        /// Государственная услуга
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ServiceId", OtherKey = "Id", CanBeNull = false)]
        public abstract Service Service { get; set; }

        /// <summary>
        /// Список этапов сценария
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ScenarioId", CanBeNull = true)]
        public abstract List<ScenarioStep> ScenarioStepList { get; set; }
    }
}
