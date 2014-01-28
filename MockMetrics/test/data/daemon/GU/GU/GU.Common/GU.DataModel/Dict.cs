using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.dict")]
    public abstract class Dict : IdentityDomainObject<Dict>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("dict_id")]
        public abstract override int Id { get; set; }

        [MapField("dict_name")]
        public abstract string Name { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "DictId", CanBeNull = false)]
        public abstract List<DictDet> DictDets { get; set; }

    }
}
