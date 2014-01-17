using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.FileScenario;

namespace PostGrad.BL.AddInList.After
{
    public class LinkageHolderAddIn : ILinkagerAddIn
    {
        private readonly ITaskParser _taskTaskParser;
        private readonly ILicenseHolderRepository _holderRepository;

        public LinkageHolderAddIn(ITaskParser taskTaskParser, ILicenseHolderRepository holderRepository)
        {
            _taskTaskParser = taskTaskParser;
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
            var holderInfo = _taskTaskParser.ParseHolderInfo(fileLink.DossierFile.Task);

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

                fileLink.LicenseHolder = _taskTaskParser.ParseHolder(fileLink.DossierFile.Task);
            }
        }
    }
}