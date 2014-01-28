using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.dict_det")]
    public abstract class DictDet : IDomainObject
    {        
        [MapField("dict_det_id")]
        public int Id { get; set; }

        [MapField("dict_id")]
        public int DictId { get; set; }

        [NoInstance]
        [Association(ThisKey = "DictId", OtherKey = "Id", CanBeNull = false)]
        public Dict Dict { get; set; }

        [MapField("item_key")]
        public string ItemKey { get; set; }

        [MapField("item_name")]
        public string ItemName { get; set; }

        [MapField("sort_order")]
        public int? SortOrder { get; set; }

        public override string ToString()
        {
            return this.ItemName;
        }
    }
}
