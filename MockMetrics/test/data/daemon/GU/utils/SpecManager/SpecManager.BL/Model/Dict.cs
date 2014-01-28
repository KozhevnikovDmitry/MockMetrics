using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.dict")]
    public class Dict : IDomainObject
    {
        [PrimaryKey]
        [MapField("dict_id")]
        public int Id { get; set; }

        [MapField("dict_name")]
        public string Name { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "DictId", CanBeNull = false)]
        public List<DictDet> DictDets { get; set; }

    }
}
