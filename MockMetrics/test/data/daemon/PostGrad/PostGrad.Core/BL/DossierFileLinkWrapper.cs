using System;
using System.Collections.Generic;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Holder;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.Core.BL
{
    public class DossierFileLinkWrapper : IDossierFileLinkWrapper
    {
        public DossierFile DossierFile { get; private set; }

        public DossierFileLinkWrapper(DossierFile dossierFile)
        {
            if (dossierFile == null) throw new ArgumentNullException("dossierFile");
            DossierFile = dossierFile;
        }

        public LicenseHolder LicenseHolder { get; set; }

        public LicenseDossier LicenseDossier { get; set; }

        public Dictionary<RequisitesOrigin, HolderRequisites> AvailableRequisites { get; set; }

        public HolderRequisites SelectedRequisites { get; set; }

        public bool IsHolderDataDoubtfull { get; set; }

        public License License { get; set; }

        public void Linkage()
        {
            if (LicenseHolder == null)
            {
                throw new NoHolderLinkagedException();
            }

            if (LicenseDossier == null)
            {
                throw new NoDossierLinkagedException();
            }

            if (SelectedRequisites == null)
            {
                throw new UnavailableRequisitesException();
            }

            DossierFile.LicenseDossier = LicenseDossier;
            DossierFile.LicenseDossierId = LicenseDossier.Id;

            DossierFile.HolderRequisites = SelectedRequisites;
            DossierFile.HolderRequisitesId = SelectedRequisites.Id;

            DossierFile.LicenseDossier.LicenseHolder = LicenseHolder;
            DossierFile.LicenseDossier.LicenseHolderId = LicenseHolder.Id;

            DossierFile.HolderRequisites.LicenseHolder = LicenseHolder;
            DossierFile.HolderRequisites.LicenseHolderId = LicenseHolder.Id;
            
            DossierFile.License = License;
            DossierFile.LicenseId = License != null ? (int?) License.Id : null;
        }
    }
}
