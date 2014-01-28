using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public class LinkageDossierAddIn : ILinkagerAddIn
    {
        private readonly LicenseDossierRepository _dossierRepository;

        public LinkageDossierAddIn(LicenseDossierRepository dossierRepository)
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