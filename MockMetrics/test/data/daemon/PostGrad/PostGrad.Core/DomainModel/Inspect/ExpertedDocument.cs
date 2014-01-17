using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Inspect
{
    [TableName("gumz.experted_document")]
    public abstract class ExpertedDocument : IdentityDomainObject<ExpertedDocument>, IPersistentObject
    {
        [PrimaryKey]
        [MapField("experted_document_id")]
        public abstract override int Id { get; set; }

        [MapField("name")]
        public abstract string Name { get; set; }

        [MapField("service_id")]
        public abstract int ServiceId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ServiceId", OtherKey = "Id", CanBeNull = false)]
        public Service Service { get; set; }
    }
}
