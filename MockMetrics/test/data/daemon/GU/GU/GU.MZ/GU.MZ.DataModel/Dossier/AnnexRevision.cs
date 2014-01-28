using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Dossier
{
    /// <summary>
    /// Класс представляющий сущность Ревизия приложения. 
    /// </summary>
    [TableName("annex_revision")]
    public abstract class AnnexRevision : IdentityDomainObject<AnnexRevision>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.annex_revision_seq")]
        [MapField("annex_revision_id")]
        public abstract override int Id { get; set; }
    }
}
