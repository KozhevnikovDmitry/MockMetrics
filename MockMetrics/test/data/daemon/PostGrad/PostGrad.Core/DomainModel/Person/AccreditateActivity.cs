using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PostGrad.Core.DomainModel.Person
{
    /// <summary>
    /// Класс для хранения данных сущности Деятельность на которую аккредитован эксперт 
    /// </summary>
    [TableName("gumz.accreditate_activity")] 
    public abstract class AccreditateActivity : IdentityDomainObject<AccreditateActivity>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.accreditate_activity_seq")]
        [MapField("accreditate_activity_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование деятельности
        /// </summary>
        [MapField("activity_name")]
        public abstract string Name { get; set; }
    }
}
