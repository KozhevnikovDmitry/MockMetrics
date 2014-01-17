using PostGrad.Core.BL;
using PostGrad.Core.DA;

namespace PostGrad.BL.AddInList.After
{
    public class CheckHolderDataAddIn : ILinkagerAddIn
    {
        private readonly ITaskParser _taskTaskParser;

        public CheckHolderDataAddIn(ITaskParser taskTaskParser)
        {
            _taskTaskParser = taskTaskParser;
        }

        public int SortOrder
        {
            get
            {
                return 4;
            }
        }

        public void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            if (fileLink.LicenseHolder == null)
            {
                fileLink.IsHolderDataDoubtfull = false;
                return;
            }

            var holderInfо = _taskTaskParser.ParseHolderInfo(fileLink.DossierFile.Task);

            fileLink.IsHolderDataDoubtfull = holderInfо.Inn != fileLink.LicenseHolder.Inn
                                             || holderInfо.Ogrn != fileLink.LicenseHolder.Ogrn;
        }
    }
}