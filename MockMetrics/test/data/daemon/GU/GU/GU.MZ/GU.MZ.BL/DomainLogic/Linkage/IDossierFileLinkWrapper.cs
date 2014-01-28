using System.Collections.Generic;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public interface IDossierFileLinkWrapper
    {
        DossierFile DossierFile { get; }
        LicenseHolder LicenseHolder { get; set; }
        LicenseDossier LicenseDossier { get; set; }
        Dictionary<RequisitesOrigin, HolderRequisites> AvailableRequisites { get; set; }
        HolderRequisites SelectedRequisites { get; set; }
        bool IsHolderDataDoubtfull { get; set; }
        License License { get; set; }
        void Linkage();
    }
}