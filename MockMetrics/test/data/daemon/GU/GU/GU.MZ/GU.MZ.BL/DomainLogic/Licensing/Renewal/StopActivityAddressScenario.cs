using System.Collections.Generic;
using System.Linq;
using GU.DataModel;
using GU.MZ.DataModel;
using GU.MZ.DataModel.LicenseChange;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Прекращение лицензируемого вида деятельности по одному или нескольким адресам
    /// </summary>
    public class StopActivityAddressScenario : BaseRenewalScenario
    {
        public StopActivityAddressScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.StopActivityAddress; }
        }

        public override ChangeLicenseSession Renewal(DossierFile dossierFile, ContentNode renewalNode)
        {
            var session = CreateSession(dossierFile);
            var license = dossierFile.License;
            var stopLicObjects = GuParser.ParseRenewalLicenseObjects(renewalNode);
            var actualObjects = license.LicenseObjectList.Where(t => t.LicenseObjectStatusId == 2).ToList();
            var unrecognizedAddresses = new List<Address>();
            foreach (var newLicObject in stopLicObjects)
            {
                bool isStopped = false;
                foreach (var actualLicObject in actualObjects)
                {
                    if (actualLicObject.AddressEquals(newLicObject))
                    {
                        isStopped = true;

                        actualLicObject.LicenseObjectStatusId = 3;
                        session.Add(actualLicObject);
                    }
                }

                if (!isStopped)
                {
                    unrecognizedAddresses.Add(newLicObject.Address);
                }
            }

            if (actualObjects.Any())
            {
                throw new UnrecognizedStopAddressesException(dossierFile, unrecognizedAddresses);
            }

            return session;
        }
        
    }
}