using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.Core.BL
{
    public interface ILinkager
    {
        IDossierFileLinkWrapper Linkage(DossierFile dossierFile);
    }
}
