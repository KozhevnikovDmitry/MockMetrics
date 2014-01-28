using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

using GU.MZ.DataModel.Person;

namespace GU.MZ.DataModel.Inspect
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Эксперт, привлечённый к выездной проверке
    /// </summary>
    [TableName("gumz.inspection_expert")] 
    public abstract class InspectionExpert : IdentityDomainObject<InspectionExpert>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.inspection_expert_seq")]
        [MapField("inspection_expert_id")] 
        public abstract override int Id { get; set; }

        /// <summary>
        /// Имя конкретного эксперта, привлечённого к проверке
        /// </summary>
        [MapField("expert_name")]
        public abstract string ExpertName { get; set; }
        
        /// <summary>
        /// Id эксперта
        /// </summary>
        [MapField("expert_id")]
        public abstract int ExpertId { get; set; }

        /// <summary>
        /// Эксперт, привлечённый к выездной проверке
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ExpertId", OtherKey = "Id", CanBeNull = false)]
        public abstract Expert Expert { get; set; }

        /// <summary>
        /// Id выездной проверки
        /// </summary>
        [MapField("inspection_id")]
        public abstract int InspectionId { get; set; }

        /// <summary>
        /// Выездная проверка
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "InspectionId", OtherKey = "Id", CanBeNull = false)]
        public abstract Inspection Inspection { get; set; }
    }
}
