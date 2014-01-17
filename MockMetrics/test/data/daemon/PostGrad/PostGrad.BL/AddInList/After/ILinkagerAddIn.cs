using PostGrad.Core.BL;
using PostGrad.Core.DA;

namespace PostGrad.BL.AddInList.After
{
    public interface ILinkagerAddIn
    {
        int SortOrder { get; }

        void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager);
    }
}