using System;
using System.Linq;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.LicenseChange;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    public abstract class LicenseObjectsScenraio : BaseRenewalScenario
    {
        protected LicenseObjectsScenraio(IParser guParser)
            : base(guParser)
        {
        }

        protected abstract string RenewalComment { get; }

        public override ChangeLicenseSession Renewal(DossierFile dossierFile, ContentNode renewalNode)
        {
            var session = CreateSession(dossierFile);
            var license = dossierFile.License;
            var newLicObjects = GuParser.ParseRenewalLicenseObjects(renewalNode);
            var actualObjects = license.LicenseObjectList.Where(t => t.LicenseObjectStatusId == 2).ToList();
            foreach (var newLicObject in newLicObjects)
            {
                bool isMerged = false;
                foreach (var actualLicObject in actualObjects)
                {
                    if (actualLicObject.AddressEquals(newLicObject))
                    {
                        isMerged = true;

                        actualLicObject.Note += string.Format("[{0} {1}: [{2}]]", DateTime.Now.ToLongDateString(),RenewalComment, newLicObject.Note);
                        session.Add(actualLicObject);
                    }
                }

                if (!isMerged)
                {
                    newLicObject.LicenseObjectStatusId = 2;
                    license.LicenseObjectList.Add(newLicObject);
                    newLicObject.LicenseId = license.Id;
                    session.Add(newLicObject);
                }
            }

            return session;
        }
    }
}


