using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.content")]
    public abstract class Content : IdentityDomainObject<Content>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu.content_content_id_seq")]
        [MapField("content_id")]
        public abstract override int Id { get; set; }

        [MapField("name")]
        public abstract string Name { get; set; }

        [MapField("spec_id")]
        public abstract int SpecId { get; set; }

        [NoInstance]
        [Association(ThisKey = "SpecId", OtherKey = "Id", CanBeNull = false)]
        public Spec Spec { get; set; }

        [MapIgnore]
        public abstract EditableList<ContentNode> RootContentNodes { get; set; }
    }
}
