using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Список категорий учета, которые указал заявитель 
    /// </summary>
    [TableName("gu_hq.declarer_base_reg")]
    public abstract class DeclarerBaseReg : IdentityDomainObject<DeclarerBaseReg>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор записи 
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.declarer_base_reg_seq")]
        [MapField("declarer_base_reg_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Id заявления
        /// </summary>
        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        /// <summary>
        /// Основание учета
        /// </summary>
        [MapField("other_base")]
        public abstract string OtherBaseReg { get; set; }

        /// <summary>
        /// списко оснований учета из справочника
        /// </summary>
        [NoInstance]
        public abstract EditableList<DeclarerBaseRegItem> BaseRegItems { get; set; }
    }
}
