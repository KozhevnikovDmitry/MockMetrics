using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.content_node")]
    public abstract class ContentNode : IDomainObject
    {
        [PrimaryKey]
        [MapField("content_node_id")]
        public int Id { get; set; }

        [MapField("content_id")]
        public int ContentId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ContentId", OtherKey = "Id", CanBeNull = false)]
        public Content Content { get; set; }

        [MapField("parent_content_node_id")]
        public int? ParentContentNodeId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ParentContentNodeId", OtherKey = "Id", CanBeNull = false)]
        public ContentNode ParentContentNode { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentContentNodeId")]
        public List<ContentNode> ChildContentNodes { get; set; }
    }
}
