using System.Linq;
using PostGrad.Core.BL;
using PostGrad.Core.DA;

namespace PostGrad.BL.AddInList.After
{
    public class LinkageLicenseAddIn : ILinkagerAddIn
    {
        private readonly ITaskParser _taskTaskParser;

        public LinkageLicenseAddIn(ITaskParser taskTaskParser)
        {
            _taskTaskParser = taskTaskParser;
        }

        public int SortOrder
        {
            get
            {
                return 3;
            }
        }

        public void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            if (fileLink.DossierFile.IsNewLicense)
            {
                return;
            }

            if (fileLink.LicenseDossier == null)
            {
                throw new NoDossierLinkagedException();
            }
            var licenseInfo = _taskTaskParser.ParseLicenseInfo(fileLink.DossierFile.Task);
            var licenses =
                fileLink.LicenseDossier.LicenseList.Where(
                    t => t.RegNumber.Trim().ToUpper() == licenseInfo.RegNumber.Trim().ToUpper()
                      && t.GrantDate == licenseInfo.GrantDate).ToList();

            if (licenses.Count > 1)
            {
                throw new TooMoreLinkagingLicesensesException(fileLink.LicenseDossier, licenseInfo.RegNumber, licenseInfo.GrantDate);
            }

            fileLink.License = licenses.SingleOrDefault();
        }
    }
}