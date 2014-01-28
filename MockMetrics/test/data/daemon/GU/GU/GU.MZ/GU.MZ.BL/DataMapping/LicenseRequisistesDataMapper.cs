using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.DataMapping
{
    public class LicenseRequisitesDataMapper : AbstractDataMapper<LicenseRequisites>
    {
        public LicenseRequisitesDataMapper(IDomainContext domainContext) : base(domainContext)
        {
        }

        protected override LicenseRequisites RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var requisites = dbManager.RetrieveDomainObject<LicenseRequisites>(id);

            if (requisites.IndRequisitesId.HasValue)
            {
                requisites.IndRequisites = dbManager.RetrieveDomainObject<IndRequisites>(requisites.IndRequisitesId);
            }
            else
            {
                requisites.JurRequisites = dbManager.RetrieveDomainObject<JurRequisites>(requisites.JurRequisitesId);
            }

            requisites.Address = dbManager.RetrieveDomainObject<Address>(requisites.AddressId);

            return requisites;
        }

        protected override LicenseRequisites SaveOperation(LicenseRequisites obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            if (tmp.IndRequisites != null)
            {
                dbManager.SaveDomainObject(tmp.IndRequisites);
                tmp.IndRequisitesId = tmp.IndRequisites.Id;
            }
            else
            {
                dbManager.SaveDomainObject(tmp.JurRequisites);
                tmp.JurRequisitesId = tmp.JurRequisites.Id;
            }

            dbManager.SaveDomainObject(tmp.Address);

            tmp.AddressId = tmp.Address.Id;

            dbManager.SaveDomainObject(tmp);

            return tmp;
        }

        protected override void FillAssociationsOperation(LicenseRequisites obj, IDomainDbManager dbManager)
        {
            
        }
    }
}
