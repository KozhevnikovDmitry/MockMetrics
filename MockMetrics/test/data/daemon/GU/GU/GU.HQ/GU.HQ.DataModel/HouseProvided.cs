
using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Предоставленное жилье
    /// </summary>
    [TableName("gu_hq.house_provided")]
    public abstract class HouseProvided : IdentityDomainObject<HouseProvided>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.house_provided_seq")]
        [MapField("house_provided_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Идентификатор заявления
        /// </summary>
        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        /// <summary>
        /// Идентификатор адреса предоставленного жилья
        /// </summary>
        [MapField("address_id")]    
        public abstract int? AddressId { get; set; }

        /// <summary>
        /// Информация о предоставленном жилье
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AddressId", OtherKey = "Id", CanBeNull = false)]
        public abstract Address Address { get; set; }

        /// <summary>
        /// отметка о внеочередном предоставлении жилья
        /// </summary>
        [MapField("is_priv_house")]
        public abstract int IsPrivHouseProvided { get; set; }

        /// <summary>
        /// основание предоставления внеочередного жилья 
        /// </summary>
        [MapField("qp_base_reg_type_id")]
        public abstract int? HouseProvidedPrivBaseTypeId { get; set; }

        /// <summary>
        /// Дата решения о предоставлении жилого помещения (78)
        /// </summary>
        [MapField("doc_date")]
        public abstract DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер решения о предоставлении жилого помещения (79)
        /// </summary>
        [MapField("doc_num")]
        public abstract string DocumentNum { get; set; }

        /// <summary>
        /// Данные о наличии жилых помещений на праве собственности после предоставления жилого помещения по договору социального найма
        /// </summary>
        [MapField("home_own")]
        public abstract string HomeOwn { get; set; }

        /// <summary>
        /// примечание (93)
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }
    }
}
