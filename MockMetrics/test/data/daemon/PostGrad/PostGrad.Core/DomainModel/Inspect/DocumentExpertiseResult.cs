using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Inspect
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Результат документарной проверки
    /// </summary>
    [TableName("gumz.doc_expertise_result")] 
    public abstract class DocumentExpertiseResult : IdentityDomainObject<DocumentExpertiseResult>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.doc_expertise_result_seq")]
        [MapField("doc_expertise_result_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Флаг указывающий на валидность документа
        /// </summary>
        [MapField("is_document_valid")]
        public abstract bool IsDocumentValid { get; set; }

        /// <summary>
        /// Комментарий о причинах невалидности документа
        /// </summary>
        [MapField("doc_invalid_note")]
        public abstract string DocumentInvalidNote { get; set; }

        /// <summary>
        /// Id документа, прилагаемого к заявке
        /// </summary>
        [MapField("experted_document_id")]
        public abstract int ExpertedDocumentId { get; set; }

        /// <summary>
        /// Документа, прилагаемый к заявке
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ExpertedDocumentId", OtherKey = "Id", CanBeNull = false)]
        public abstract ExpertedDocument ExpertedDocument { get; set; }

        /// <summary>
        /// Id проверки
        /// </summary>
        [MapField("file_scenario_step_id")]
        public abstract int DocumentExpertiseId { get; set; }

        /// <summary>
        /// Документарная проверка
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "DocumentExpertiseId", OtherKey = "Id", CanBeNull = false)]
        public abstract DocumentExpertise DocumentExpertise { get; set; }
    }
}
