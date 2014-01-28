using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public interface ILinkager
    {
        IDossierFileLinkWrapper Linkage(DossierFile dossierFile);
    }
}
