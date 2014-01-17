using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;

namespace PostGrad.BL.AddInList.After
{
    public class LinkageDossierAddIn : ILinkagerAddIn
    {
        private readonly ILicenseDossierRepository _dossierRepository;

        public LinkageDossierAddIn(ILicenseDossierRepository dossierRepository)
        {
            _dossierRepository = dossierRepository;
        }

        public int SortOrder
        {
            get
            {
                return 2;
            }
        }

        public void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            if (_dossierRepository.DossierExists(fileLink.DossierFile.LicensedActivity.Id,
                                             fileLink.LicenseHolder.Id, dbManager))
            {
                fileLink.LicenseDossier = _dossierRepository.GetLicenseDossier(fileLink.DossierFile.LicensedActivity.Id,
                                                                  fileLink.LicenseHolder.Id, dbManager);
            }
            else
            {
                if (fileLink.DossierFile.ScenarioType == ScenarioType.Light)
                {
                    throw new NoExistingDossierForLightSceanrioException();
                }

                fileLink.LicenseDossier = LicenseDossier.CreateInstance(fileLink.DossierFile.LicensedActivity, fileLink.LicenseHolder);
            }
        }
    }
}