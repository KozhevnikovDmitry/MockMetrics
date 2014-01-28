using System;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.Archive.DataModel;

namespace GU.Archive.BL.DataMapping
{
    public class OrganizationDataMapper : AbstractDataMapper<Organization>
    {
        public OrganizationDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override Organization RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            Organization rec = dbManager.RetrieveDomainObject<Organization>(id);

            if (rec.AddressId != 0)
            {
                rec.Address = dbManager.RetrieveDomainObject<Address>(rec.AddressId);
            }

            return rec;
        }

        protected override Organization SaveOperation(Organization obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            if (tmp.Address != null)
            {
                dbManager.SaveDomainObject(tmp.Address);
                tmp.AddressId = tmp.Address.Id;
            }

            dbManager.SaveDomainObject(tmp);
            return tmp;
        }

        protected override void FillAssociationsOperation(Organization obj, IDomainDbManager dbManager)
        {
            if (obj.AddressId != 0)
            {
                obj.Address = dbManager.RetrieveDomainObject<Address>(obj.AddressId);
            }
        }
    }
}
