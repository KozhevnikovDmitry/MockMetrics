using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.BL.AddInList.After
{
    public class SetupFileAddIn : ILinkagerAddIn
    {
        private readonly ILicenseDossierRepository _dossierRepository;

        public SetupFileAddIn(ILicenseDossierRepository dossierRepository)
        {
            _dossierRepository = dossierRepository;
        }

        public int SortOrder
        {
            get
            {
                return 5;
            }
        }

        public void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            if (fileLink.LicenseDossier == null)
            {
                throw new NoDossierLinkagedException();
            }

            fileLink.DossierFile.RegNumber = _dossierRepository.GetNextFileRegNumber(fileLink.LicenseDossier, dbManager);
            fileLink.DossierFile.CurrentStatus = DossierFileStatus.Active;
        }
    }
}