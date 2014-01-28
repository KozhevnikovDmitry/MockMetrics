using System;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.LicenseChange;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    public abstract class ChangeRequisitesScenario : BaseRenewalScenario
    {
        protected ChangeRequisitesScenario(IParser guParser) : base(guParser)
        {
        }

        public override ChangeLicenseSession Renewal(DossierFile dossierFile, ContentNode renewalNode)
        {
            var session = CreateSession(dossierFile);
            var parsedRequisites = GuParser.ParseHolder(dossierFile.Task).ActualRequisites;
            parsedRequisites.CreateDate = DateTime.Now;
            var licenseRequisites = parsedRequisites.ToLicenseRequisites();
            licenseRequisites.LicenseId = dossierFile.License.Id;
            session.Add(licenseRequisites);
            dossierFile.License.LicenseRequisitesList.Add(parsedRequisites.ToLicenseRequisites());
            return session;
        }
    }
}