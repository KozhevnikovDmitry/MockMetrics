using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    [TableName("gu_hq.declarer_base_reg_item")]
    public abstract class DeclarerBaseRegItem : IdentityDomainObject<DeclarerBaseRegItem>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор записи об информации основания подачи зачявления
        /// </summary>
        [PrimaryKey,Identity]
        [SequenceName("gu_hq.declarer_base_reg_item_seq ")]
        [MapField("declarer_base_reg_item_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// идентификатор объекта "информация об основании учета указанная заявителем"
        /// </summary>
        [MapField("declarer_base_reg_id")]
        public abstract int DeclarerBaseRegId { get; set; }

        /// <summary>
        /// Основание учета заявления
        /// </summary>
        [NoInstance]
        public abstract QueueBaseRegType QueueBaseRegType { get; set; }

        /// <summary>
        /// Пункт основания учета
        /// </summary>
        [MapField("q_base_reg_type_id")]
        public abstract int QueueBaseRegTypeId { get; set; }
    }
}
