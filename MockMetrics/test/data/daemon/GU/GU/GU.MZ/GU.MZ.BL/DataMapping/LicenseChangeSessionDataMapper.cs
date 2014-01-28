using System;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.DataModel.LicenseChange;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DataMapping
{
    public class LicenseChangeSessionDataMapper : AbstractDataMapper<ChangeLicenseSession>
    {
        private readonly IDomainDataMapper<LicenseRequisites> _licRequisitesMapper;
        private readonly IDomainDataMapper<License> _licenseMapper;
        private readonly IDomainDataMapper<LicenseObject> _licObjMapper;

        public LicenseChangeSessionDataMapper(IDomainContext domainContext, 
                                              IDomainDataMapper<LicenseRequisites> licRequisitesMapper, 
                                              IDomainDataMapper<License> licenseMapper, 
                                              IDomainDataMapper<LicenseObject> licObjMapper)
            : base(domainContext)
        {
            _licRequisitesMapper = licRequisitesMapper;
            _licenseMapper = licenseMapper;
            _licObjMapper = licObjMapper;
        }

        protected override ChangeLicenseSession RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var session = dbManager.RetrieveDomainObject<ChangeLicenseSession>(id);
            var requisitesIds =
               dbManager.GetDomainTable<ChangedLicenseRequisites>()
                        .Where(t => t.ChangeLicenseSessionId == session.Id)
                        .Select(t => t.Id)
                        .ToList();

            foreach (var requisitesId in requisitesIds)
            {
                var changedLicenseRequisites = dbManager.RetrieveDomainObject<ChangedLicenseRequisites>(requisitesId);
                changedLicenseRequisites.ChangeLicenseSession = session;
                changedLicenseRequisites.LicenseRequisites =
                    _licRequisitesMapper.Retrieve(changedLicenseRequisites.LicenseRequisitesId, dbManager);
                session.ChangedLicenseRequisitesList.Add(changedLicenseRequisites);
            }

            var licObjIds =
               dbManager.GetDomainTable<ChangedLicenseObject>()
                        .Where(t => t.ChangeLicenseSessionId == session.Id)
                        .Select(t => t.Id)
                        .ToList();

            foreach (var licObjId in licObjIds)
            {
                var changedLicenseObject = dbManager.RetrieveDomainObject<ChangedLicenseObject>(licObjId);
                changedLicenseObject.ChangeLicenseSession = session;
                changedLicenseObject.LicenseObject =
                    _licObjMapper.Retrieve(changedLicenseObject.LicenseObjectId, dbManager);
                session.ChangedLicenseObjectList.Add(changedLicenseObject);
            }

            var licIds =
               dbManager.GetDomainTable<ChangedLicense>()
                        .Where(t => t.ChangeLicenseSessionId == session.Id)
                        .Select(t => t.Id)
                        .ToList();

            foreach (var licId in licIds)
            {
                var changedLicense = dbManager.RetrieveDomainObject<ChangedLicense>(licId);
                changedLicense.ChangeLicenseSession = session;
                changedLicense.License =
                    _licenseMapper.Retrieve(changedLicense.LicenseId, dbManager);
                session.ChangedLicenseList.Add(changedLicense);
            }

            return session;
        }

        protected override ChangeLicenseSession SaveOperation(ChangeLicenseSession obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp);

            foreach (var changedLicenseRequisites in tmp.ChangedLicenseRequisitesList)
            {
                changedLicenseRequisites.LicenseRequisites =
                    _licRequisitesMapper.Save(changedLicenseRequisites.LicenseRequisites, dbManager);
                changedLicenseRequisites.LicenseRequisitesId = changedLicenseRequisites.LicenseRequisites.Id;
                changedLicenseRequisites.ChangeLicenseSessionId = tmp.Id;
                dbManager.SaveDomainObject(changedLicenseRequisites);
            }

            foreach (var changedLicenseObject in tmp.ChangedLicenseObjectList)
            {
                changedLicenseObject.LicenseObject =
                    _licObjMapper.Save(changedLicenseObject.LicenseObject, dbManager);
                changedLicenseObject.LicenseObjectId = changedLicenseObject.LicenseObject.Id;
                changedLicenseObject.ChangeLicenseSessionId = tmp.Id;
                dbManager.SaveDomainObject(changedLicenseObject);
            }

            foreach (var changedLicense in tmp.ChangedLicenseList)
            {
                changedLicense.License =
                    _licenseMapper.Save(changedLicense.License, dbManager);
                changedLicense.LicenseId = changedLicense.License.Id;
                changedLicense.ChangeLicenseSessionId = tmp.Id;
                dbManager.SaveDomainObject(changedLicense);
            }

            return tmp;
        }

        protected override void FillAssociationsOperation(ChangeLicenseSession obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}