using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    public interface IContentMapper
    {
        LicenseHolder MapHolder(ContentNode holderNode);

        HolderRequisites MapRequisites(ContentNode holderNode);

        Address MapAddress(ContentNode addressNode);
        HolderInfo MapHolderInfo(ContentNode holderNode);
        LicenseInfo MapLicenseInfo(ContentNode licenseNode);
        HolderRequisites MapJurRenewalRequisites(ContentNode renewalNode);
        HolderRequisites MapIndRenewalRequisites(ContentNode renewalNode);
    }
}