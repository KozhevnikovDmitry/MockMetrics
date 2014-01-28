using System.Collections.Generic;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.content")]
    public class Content : IDomainObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu.content_content_id_seq")]
        [MapField("content_id")]
        public int Id { get; set; }

        [MapField("spec_id")]
        public int SpecId { get; set; }

        [NoInstance]
        [Association(ThisKey = "SpecId", OtherKey = "Id", CanBeNull = false)]
        public Spec Spec { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ContentId")]
        public List<ContentNode> ChildContentNodes { get; set; }
    }
}
