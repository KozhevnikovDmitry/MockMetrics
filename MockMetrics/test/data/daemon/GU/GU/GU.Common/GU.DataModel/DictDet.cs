using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.dict_det")]
    public abstract class DictDet : IdentityDomainObject<DictDet>, IPersistentObject
    {        
        [MapField("dict_det_id")]
        public abstract override int Id { get; set; }

        [MapField("dict_id")]
        public abstract int DictId { get; set; }

        [NoInstance]
        [Association(ThisKey = "DictId", OtherKey = "Id", CanBeNull = false)]
        public abstract Dict Dict { get; set; }

        [MapField("item_key")]
        public abstract string ItemKey { get; set; }

        [MapField("item_name")]
        public abstract string ItemName { get; set; }

        [MapField("sort_order")]
        public abstract int? SortOrder { get; set; }

        public override string ToString()
        {
            return ItemName;
        }
    }
}
