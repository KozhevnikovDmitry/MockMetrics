using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;

using GU.DataModel;

namespace GU.MZ.DataModel.Inspect
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
