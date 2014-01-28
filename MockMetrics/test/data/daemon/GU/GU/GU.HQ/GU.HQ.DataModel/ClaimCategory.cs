
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Категории указанные в заявлении заявителем
    /// </summary>
    [TableName("gu_hq.claim_category")]
    public abstract class ClaimCategory : IdentityDomainObject<ClaimCategory>, IPersistentObject 
    {
        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.claim_category_seq")]
        [MapField("claim_category_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Идентификатор заявления
        /// </summary>
        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }


        /// <summary>
        /// Идентификатор категории
        /// </summary>
        [MapField("category_type_id")]
        public abstract int CategoryTypeId { get; set; }


        /// <summary>
        /// Описание категории заявления
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "CategoryId", OtherKey = "Id", CanBeNull = false)]
        public abstract CategoryType Category { get; set; }
    }
}
