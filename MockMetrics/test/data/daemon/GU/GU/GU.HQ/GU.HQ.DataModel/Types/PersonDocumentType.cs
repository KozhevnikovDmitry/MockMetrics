using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel.Types
{
    [TableName("gu_hq.person_doc_type")]
    public abstract class PersonDocumentType : IdentityDomainObject<PersonDocumentType>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.person_doc_type_seq")]
        [MapField("person_doc_type_id")]
        public abstract override int Id { get; set; }

        [MapField("name")]
        public abstract string Name { get; set; }


        public override string ToString() { return Name; }
    }
}
