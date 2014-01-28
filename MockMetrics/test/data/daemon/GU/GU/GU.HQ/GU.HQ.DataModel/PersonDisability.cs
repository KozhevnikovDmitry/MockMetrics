using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    [TableName("gu_hq.person_disability")]
    public abstract class PersonDisability : IdentityDomainObject<PersonDisability>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.person_disability_seq")]
        [MapField("person_disability_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Идентификатор Person
        /// </summary>
        [MapField("person_id")]
        public abstract int PersonId { get; set; }

        /// <summary>
        /// Группа инвалидности
        /// </summary>
        [MapField("disability_type_id")]
        public abstract int DisabilityTypeId { get; set; }

        /// <summary>
        /// Дата по которую установлена инвалидность
        /// </summary>
        [MapField("disability_date")]
        public abstract DateTime? DisabilityDate { get; set; }
        
        /// <summary>
        /// Комментарий
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set;  }

        /// <summary>
        /// Отметка: малоимущий
        /// </summary>
        [MapField("is_poor")]
        public abstract int IsPoor { get; set; }

        /// <summary>
        /// Номер решения управления социальной защиты населения
        /// </summary>
        [MapField("is_poor_doc_num")]
        public abstract string IsPoorDocNum { get; set; }

        /// <summary>
        /// Дата решения управления социальной защиты населения 
        /// </summary>
        [MapField("is_poor_doc_date")]
        public abstract DateTime? IsPoorDocDate { get; set; }

        /// <summary>
        /// Номер решения управления социальной защиты населения
        /// </summary>
        [MapField("uszn_doc_num")]
        public abstract string UsznDocNum { get; set; }

        /// <summary>
        /// Дата решения управления социальной защиты населения 
        /// </summary>
        [MapField("uszn_doc_date")]
        public abstract DateTime? UsznDocDate { get; set; }
    }
}
