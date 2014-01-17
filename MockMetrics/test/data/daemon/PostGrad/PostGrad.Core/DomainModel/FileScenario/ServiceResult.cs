using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.FileScenario
{
    /// <summary>
    /// Класс для хранения данных сущности Результат предоставления услуги
    /// </summary>
    [TableName("gumz.service_result")]
    public abstract class ServiceResult : IdentityDomainObject<ServiceResult>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [MapField("service_result_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование результат
        /// </summary>
        [MapField("result_name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Флаг положительного\отрицательного результата
        /// </summary>
        [MapField("is_positive")]
        public abstract bool IsPositive { get; set; }

        /// <summary>
        /// Id сущности Сценарий ведения тома
        /// </summary>
        [MapField("scenario_id")]
        public abstract int ScenarioId { get; set; }

        /// <summary>
        /// Сценарий ведения тома
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ScenarioId", OtherKey = "Id", CanBeNull = false)]
        public abstract Scenario Scenario { get; set; }
    }
}
