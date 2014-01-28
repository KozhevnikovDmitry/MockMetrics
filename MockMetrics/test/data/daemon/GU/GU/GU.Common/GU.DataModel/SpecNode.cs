using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.spec_node")]
    public abstract class SpecNode : IdentityDomainObject<SpecNode>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("spec_node_id")]
        public abstract override int Id { get; set; }

        [MapField("spec_id")]
        public abstract int SpecId { get; set; }

        [NoInstance]
        [Association(ThisKey = "SpecId", OtherKey = "Id", CanBeNull = false)]
        public Spec Spec { get; set; }

        [MapField("parent_spec_node_id")]
        public abstract int? ParentSpecNodeId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ParentSpecNodeId", OtherKey = "Id", CanBeNull = false)]
        public SpecNode ParentSpecNode { get; set; }

        [MapField("name")]
        public abstract string Name { get; set; }

        [MapField("tag")]
        public abstract string Tag { get; set; }

        [MapField("spec_node_type_id")]
        public abstract SpecNodeType SpecNodeType { get; set; }

        [MapField("attr_data_type_id")]
        public abstract AttrDataType? AttrDataType { get; set; }

        [MapField("min_occurs")]
        public abstract int MinOccurs { get; set; }

        [MapField("max_occurs")]
        public abstract int? MaxOccurs { get; set; }

        [MapField("ref_spec_id")]
        public abstract int? RefSpecId { get; set; }
        
        [NoInstance]
        [Association(ThisKey = "RefSpecId", OtherKey = "Id", CanBeNull = false)]
        public Spec RefSpec { get; set; }

        [MapField("format_regexp")]
        public abstract string FormatRegexp { get; set; }

        [MapField("format_description")]
        public abstract string FormatDescription { get; set; }

        [MapField("dict_id")]
        public abstract int? DictId { get; set; }

        [NoInstance]
        [Association(ThisKey = "DictId", OtherKey = "Id", CanBeNull = true)]
        public abstract Dict Dict { get; set; }

        [MapField("is_editable_dict")]
        public abstract bool? IsEditableDict { get; set; }

        [MapField("sort_order")]
        public abstract int? Order { get; set; }

        [MapField("note")]
        public abstract string Note { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentSpecNodeId")]
        public List<SpecNode> ChildSpecNodes { get; set; }

        [MapIgnore]
        [CloneIgnore]
        public bool IsRequered
        {
            get
            {
                return MinOccurs > 0;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public bool IsSingleOrNull
        {
            get
            {
                return MinOccurs == 0 && MaxOccurs.HasValue && MaxOccurs.Value == 1;
            }
        }

    }
}
