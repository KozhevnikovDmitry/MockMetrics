using System.Collections.Generic;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Holder;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.Core.BL
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