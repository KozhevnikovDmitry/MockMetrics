using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public class CheckHolderDataAddIn : ILinkagerAddIn
    {
        private readonly IParser _taskParser;

        public CheckHolderDataAddIn(IParser taskParser)
        {
            _taskParser = taskParser;
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

            var holderInfо = _taskParser.ParseHolderInfo(fileLink.DossierFile.Task);

            fileLink.IsHolderDataDoubtfull = holderInfо.Inn != fileLink.LicenseHolder.Inn
                                             || holderInfо.Ogrn != fileLink.LicenseHolder.Ogrn;
        }
    }
}