using System;
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
    /// Прекращение выполнения работ, оказания услуг
    /// </summary>
    public class StopWorksScenario : BaseRenewalScenario
    {
        public StopWorksScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.StopWorks; }
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
                        actualLicObject.Note += string.Format("[{0} Прекращена деятельность: [{1}]]", DateTime.Now.ToLongDateString(), newLicObject.Note);
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