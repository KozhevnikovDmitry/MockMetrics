using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Dossier
{
    [TableName("gumz.provided_document")]
    public abstract class ProvidedDocument : IdentityDomainObject<ProvidedDocument>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.provided_document_seq")]
        [MapField("provided_document_id")]
        public abstract override int Id { get; set; }

        [MapField("document_name")]
        public abstract string Name { get; set; }

        [MapField("quantity")]
        public abstract int Quantity { get; set; }

        [MapField("doc_inventory_id")]
        public abstract int DocumentInventoryId { get; set; }

        [NoInstance]
        [Association(ThisKey = "DocumentInventoryId", OtherKey = "Id", CanBeNull = false)]
        public DocumentInventory DocumentInventory { get; set; }
    }
}
