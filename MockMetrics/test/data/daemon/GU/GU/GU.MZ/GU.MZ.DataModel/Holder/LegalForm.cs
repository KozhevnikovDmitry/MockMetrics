using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Holder
{
    /// <summary>
    /// Класс представляющий сущность Организационно-правовая форма.
    /// </summary>
    [TableName("gumz.legal_form")]
    public abstract class LegalForm : IdentityDomainObject<LegalForm>, IPersistentObject
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        [PrimaryKey, Identity]
        [MapField("legal_form_id")]
        [SequenceName("gumz.legal_form_seq")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Наименование организационно-правовой формы
        /// </summary>
        [MapField("legal_form_name")]
        public abstract string Name { get; set; }
    }
}
