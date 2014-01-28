using System.Collections.Generic;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    public interface ILicenseRenewaller
    {
        License RenewalLicense(DossierFile dossierFile);

        DossierFile SaveChanges(DossierFile dossierFile);

        bool IsRenewalled(DossierFile dossierFile);

        List<string> RenewalScenaries(DossierFile dossierFile);
    }
}
