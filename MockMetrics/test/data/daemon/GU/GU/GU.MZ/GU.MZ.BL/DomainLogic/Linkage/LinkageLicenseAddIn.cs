using System.Linq;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public class LinkageLicenseAddIn : ILinkagerAddIn
    {
        private readonly IParser _taskParser;
        private readonly IDomainDataMapper<License> _licenseMapper;

        public LinkageLicenseAddIn(IParser taskParser, IDomainDataMapper<License> licenseMapper)
        {
            _taskParser = taskParser;
            _licenseMapper = licenseMapper;
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
            var licenseInfo = _taskParser.ParseLicenseInfo(fileLink.DossierFile.Task);
            var licenses =
                fileLink.LicenseDossier.LicenseList.Where(
                    t => t.RegNumber.Trim().ToUpper() == licenseInfo.RegNumber.Trim().ToUpper()
                      && t.GrantDate == licenseInfo.GrantDate).ToList();

            if (licenses.Count > 1)
            {
                throw new TooMoreLinkagingLicesensesException(fileLink.LicenseDossier, licenseInfo.RegNumber, licenseInfo.GrantDate);
            }

            fileLink.License = licenses.SingleOrDefault() != null ? _licenseMapper.Retrieve(licenses.Single().Id, dbManager) : null; ;
        }
    }
}