using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.LicenseChange;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    public interface IRenewalScenario
    {
        RenewalType RenewalType { get; }

        ChangeLicenseSession Renewal(DossierFile dossierFile, ContentNode renewalNode);
    }

    public abstract class BaseRenewalScenario : IRenewalScenario
    {
        protected IParser GuParser { get; set; }

        protected BaseRenewalScenario(IParser guParser)
        {
            GuParser = guParser;
        }

        public abstract RenewalType RenewalType { get; }

        public abstract ChangeLicenseSession Renewal(DossierFile dossierFile, ContentNode renewalNode);

        protected ChangeLicenseSession CreateSession(DossierFile dossierFile)
        {
            var session = ChangeLicenseSession.CreateInstance();
            session.DossierFile = dossierFile;
            session.DossierFileId = dossierFile.Id;
            session.License = dossierFile.License;
            session.LicenseId = dossierFile.License.Id;
            return session;
        }
    }
}
