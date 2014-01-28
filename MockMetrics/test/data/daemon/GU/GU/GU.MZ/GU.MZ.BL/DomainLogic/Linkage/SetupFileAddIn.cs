using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public class SetupFileAddIn : ILinkagerAddIn
    {
        private readonly LicenseDossierRepository _dossierRepository;

        public SetupFileAddIn(LicenseDossierRepository dossierRepository)
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