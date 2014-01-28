using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.FileScenario
{
    /// <summary>
    /// Класс, представляющий сущность Этап сценария ведения тома
    /// </summary>
    [TableName("gumz.scenario_step")]
    public abstract class ScenarioStep : IdentityDomainObject<ScenarioStep>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [MapField("scenario_step_id")] 
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование этапа.
        /// </summary>
        [MapField("scenario_step_name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Порядковый номер этапа в сценарии
        /// </summary>
        [MapField("sort_order")]
        public abstract int SortOrder { get; set; }
        
        /// <summary>
        /// Id сущности Сценарий ведения тома
        /// </summary>
        [MapField("scenario_id")]
        public abstract int ScenarioId { get; set; }

        [MapField("due_days")]
        public abstract int DueDays { get; set; }

        /// <summary>
        /// Сценарий ведения тома
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ScenarioId", OtherKey = "Id", CanBeNull = false)]
        public abstract FileScenario.Scenario Scenario { get; set; }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}
