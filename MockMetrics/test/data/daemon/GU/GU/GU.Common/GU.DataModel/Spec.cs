using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.spec")]
    public abstract class Spec : IdentityDomainObject<Spec>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("spec_id")]
        public abstract override int Id { get; set; }

        [MapField("spec_name")]
        public abstract string Name { get; set; }

        [MapField("spec_uri")]
        public abstract string Uri { get; set; }

        //[Association(ThisKey = "Id", OtherKey = "SpecId")]
        //public List<SpecNode> Elements { get; set; }

        [MapIgnore]
        public List<SpecNode> RootSpecNodes { get; set; }
    }
}
