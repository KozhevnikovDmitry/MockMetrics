using Common.DA.Interface;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public interface ILinkagerAddIn
    {
        int SortOrder { get; }

        void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager);
    }
}