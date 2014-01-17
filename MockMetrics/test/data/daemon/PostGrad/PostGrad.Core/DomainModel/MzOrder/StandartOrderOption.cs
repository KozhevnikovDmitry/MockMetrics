using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PostGrad.Core.DomainModel.MzOrder
{
    /// <summary>
    /// Настройка типового приказа
    /// Настраивает отчёт для печати приказов:
    /// 1 - Возврат заявления
    /// 2 - Принятие к рассмотрению
    /// 3 - Предоставление лицензии
    /// 4 - Отказ в предоставлении лицензии
    /// 5 - Переоформление лицензии
    /// 6 - Отказ в переоформлении лицензии
    /// 7 - Прекращение действия лицензии
    /// </summary>
    [TableName("gumz.standart_order_option")]
    public abstract class StandartOrderOption : IdentityDomainObject<StandartOrderOption>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.standart_order_option_seq")]
        [MapField("standart_order_option_id")]
        public abstract override int Id { get; set; }

        [MapIgnore]
        [CloneIgnore]
        public StandartOrderType OrderType
        {
            get
            {
                return (StandartOrderType) Id;
            }
        }

        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Преамбула приказа
        /// </summary>
        [MapField("preamble")]
        public abstract string Preamble { get; set; }

        /// <summary>
        /// Преамбула приложения к приказу
        /// </summary>
        [MapField("annex_preamble")]
        public abstract string AnnexPreamble { get; set; }

        /// <summary>
        /// Заголовок к детализации приказа - например "Номер и дата регистрации лицензии" или "Номер и дата регистрации заявления и прилагаемых к нему документов"
        /// </summary>
        [MapField("order_detail_header")]
        public abstract string OrderDetailHeader { get; set; }

        /// <summary>
        /// Шаблон комментария к детализации приказа - например "Обоснование причин отказа в предоставлении лицензии: {0}"
        /// </summary>
        [MapField("deatil_comment_pattern")]
        public abstract string DetailCommentPattern { get; set; }

        /// <summary>
        /// Название id предмета детализации - например "Номер заявления" или "Номер лицензии"
        /// </summary>
        [MapField("subject_id_name")]
        public abstract string SubjectIdName { get; set; }

        /// <summary>
        /// Название даты предмета детализации - примере "Дата подачи" или "Дата регистрации"
        /// </summary>
        [MapField("subject_stamp_name")]
        public abstract string SubjectStampName { get; set; }   
    }
}