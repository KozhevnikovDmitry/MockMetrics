
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel.Types
{
    /// <summary>
    /// Категории учета. Указываются заявителем при подаче заявления
    /// </summary>
    [TableName("gu_hq.category_type")]
    public abstract class CategoryType : IdentityDomainObject<CategoryType>, IPersistentObject
    {
        /// <summary>
        /// идентификатор в справочнике
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.category_type_seq")]
        [MapField("category_type_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// наименование категории учета
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Идентификатор ротельской категории
        /// </summary>
        [MapField("parent_category_type_id")]
        public abstract string ParentCategoryId { get; set; }

        /// <summary>
        /// Список подкатегорий данной категории
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ParentCategoryId", CanBeNull = true)]
        public abstract EditableList<CategoryType> Categories { get; set; }
    }
}
