using BLToolkit.EditableObjects;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.Core.BL
{
    public interface ILicenseObjectProvider
    {
        EditableList<LicenseObject> GetNewLicenseObjects(Task task);
    }
}