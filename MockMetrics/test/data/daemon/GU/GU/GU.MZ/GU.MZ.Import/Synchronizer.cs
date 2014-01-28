using System.Linq;
using Common.DA.Interface;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.Import
{
    public class Synchronizer
    {
        public HolderRequisites SyncRequisites(HolderRequisites reqs, IDomainDbManager db)
        {
            var equivalent = db.GetDomainTable<HolderRequisites>()
                .SingleOrDefault(t => t.LicenseHolderId == reqs.LicenseHolderId
                                   && t.CreateDate == reqs.CreateDate
                                   && t.JurRequisitesId.HasValue
                                   && t.JurRequisites.FirmName == reqs.JurRequisites.FirmName
                                   && t.JurRequisites.FullName == reqs.JurRequisites.FullName
                                   && t.JurRequisites.ShortName == reqs.JurRequisites.ShortName
                                   && t.JurRequisites.HeadName == reqs.JurRequisites.HeadName
                                   && t.JurRequisites.HeadPositionName == reqs.JurRequisites.HeadPositionName
                                   && t.JurRequisites.LegalFormId == reqs.JurRequisites.LegalFormId
                                   && t.Note == reqs.Note);

            var result = equivalent != null ? db.RetrieveDomainObject<HolderRequisites>(equivalent.Id) : reqs;

            if (result.JurRequisitesId.HasValue)
            {
                result.JurRequisites = db.RetrieveDomainObject<JurRequisites>(result.JurRequisitesId);
            }
            else
            {
                if (result.IndRequisitesId.HasValue)
                {
                    result.IndRequisites = db.RetrieveDomainObject<IndRequisites>(result.IndRequisitesId);
                }
            }

            return result;
        }

        public Address SyncHolderAddress(HolderRequisites requisites, Address address, IDomainDbManager db)
        {
            return SyncAddress(requisites.Id, requisites.AddressId, address, db);
        }

        public LicenseRequisites SyncRequisites(LicenseRequisites reqs, IDomainDbManager db)
        {
            var equivalent = db.GetDomainTable<LicenseRequisites>()
                .SingleOrDefault(t => t.LicenseId == reqs.LicenseId
                                   && t.CreateDate == reqs.CreateDate
                                   && t.JurRequisites.FirmName == reqs.JurRequisites.FirmName
                                   && t.JurRequisites.FullName == reqs.JurRequisites.FullName
                                   && t.JurRequisites.ShortName == reqs.JurRequisites.ShortName
                                   && t.JurRequisites.HeadName == reqs.JurRequisites.HeadName
                                   && t.JurRequisites.HeadPositionName == reqs.JurRequisites.HeadPositionName
                                   && t.JurRequisites.LegalFormId == reqs.JurRequisites.LegalFormId
                                   && t.Note == reqs.Note);

            var result = equivalent != null ? db.RetrieveDomainObject<LicenseRequisites>(equivalent.Id) : reqs;

            if (result.JurRequisitesId.HasValue)
            {
                result.JurRequisites = db.RetrieveDomainObject<JurRequisites>(result.JurRequisitesId);
            }
            else
            {
                if (result.IndRequisitesId.HasValue)
                {
                    result.IndRequisites = db.RetrieveDomainObject<IndRequisites>(result.IndRequisitesId);
                }
            }

            return result;
        }

        public LicenseObject SyncLicenseObject(LicenseObject licObj, IDomainDbManager db)
        {
            var equivalent = db.GetDomainTable<LicenseObject>()
                .SingleOrDefault(t => t.LicenseId == licObj.LicenseId
                                      && t.LicenseObjectStatusId == licObj.LicenseObjectStatusId
                                      && t.Name == licObj.Name
                                      && t.Note == licObj.Note
                                      && t.GrantOrderRegNumber == licObj.GrantOrderRegNumber
                                      && t.GrantOrderStamp == licObj.GrantOrderStamp
                                      && t.Address.Zip == licObj.Address.Zip
                                      && t.Address.Country == licObj.Address.Country
                                      && t.Address.CountryRegion == licObj.Address.CountryRegion
                                      && t.Address.Area == licObj.Address.Area
                                      && t.Address.City == licObj.Address.City
                                      && t.Address.Street == licObj.Address.Street
                                      && t.Address.House == licObj.Address.House
                                      && t.Address.Build == licObj.Address.Build
                                      && t.Address.Flat == licObj.Address.Flat
                                      && t.Address.Note == licObj.Address.Note);
                                   
            return equivalent != null ? db.RetrieveDomainObject<LicenseObject>(equivalent.Id) : licObj;
        }

        public Address SyncLicenseObjectAddress(LicenseObject licenseObject, Address address, IDomainDbManager db)
        {
            return SyncAddress(licenseObject.Id, licenseObject.AddressId, address, db);
        }

        public Address SyncLicenseAddress(LicenseRequisites requisites, Address address, IDomainDbManager db)
        {
            return SyncAddress(requisites.Id, requisites.AddressId, address, db);
        }

        private Address SyncAddress(int parentId, int? addressId, Address address, IDomainDbManager db)
        {
            if (parentId == 0 || addressId == 0)
            {
                return address;
            }

            var equivalent = db.RetrieveDomainObject<Address>(addressId);

            if (equivalent.Zip == address.Zip &&
                equivalent.Country == address.Country &&
                equivalent.CountryRegion == address.CountryRegion &&
                equivalent.Area == address.Area &&
                equivalent.City == address.City &&
                equivalent.Street == address.Street &&
                equivalent.House == address.House &&
                equivalent.Build == address.Build &&
                equivalent.Flat == address.Flat &&
                equivalent.Note == address.Note)
            {
                return equivalent;
            }

            return address;
        }
    }
}
