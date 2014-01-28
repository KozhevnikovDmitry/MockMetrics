using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.FileScenario;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public class LinkageHolderAddIn : ILinkagerAddIn
    {
        private readonly IParser _taskParser;
        private readonly LicenseHolderRepository _holderRepository;

        public LinkageHolderAddIn(IParser taskParser, LicenseHolderRepository holderRepository)
        {
            _taskParser = taskParser;
            _holderRepository = holderRepository;
        }

        public int SortOrder
        {
            get
            {
                return 0;
            }
        }

        public void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            var holderInfo = _taskParser.ParseHolderInfo(fileLink.DossierFile.Task);

            if (_holderRepository.HolderExists(holderInfo.Inn, holderInfo.Ogrn, dbManager))
            {
                fileLink.LicenseHolder = _holderRepository.GetLicenseHolder(holderInfo.Inn, holderInfo.Ogrn, dbManager);
            }
            else
            {
                if (fileLink.DossierFile.ScenarioType == ScenarioType.Light)
                {
                    throw new NoExistingHolderForLightSceanrioException();
                }

                fileLink.LicenseHolder = _taskParser.ParseHolder(fileLink.DossierFile.Task);
            }
        }
    }
}