using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Документы
    /// </summary>
    [TableName("gu_hq.person_doc")]
    public abstract class PersonDoc : IdentityDomainObject<PersonDoc>, IPersistentObject
    {
        /// <summary>
        /// идентификатор строки
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.person_doc_seq")]
        [MapField("person_doc_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Тип документа
        /// </summary>
        [MapField("person_doc_type_id")]
        public abstract int DocumentTypeId { get; set; }

        /// <summary>
        /// владелец документа
        /// </summary>
        [MapField("person_id")]
        public abstract int PersonId { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        [MapField("seria")]
        public abstract string Seria { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        [MapField("num")]
        public abstract string Num { get; set; }


        /// <summary>
        /// дата выдачи документа
        /// </summary>
        [MapField("date_doc")]
        public abstract DateTime DateDoc { get; set; }

        /// <summary>
        /// организация которая выдала документ
        /// </summary>
        [MapField("org")]
        public abstract string Org { get; set; }


        /// <summary>
        /// код организации
        /// </summary>
        [MapField("org_code")]
        public abstract string OrgCode { get; set; }


        /// <summary>
        /// комментарий
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }

    }
}
