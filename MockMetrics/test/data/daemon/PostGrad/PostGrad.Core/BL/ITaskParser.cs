using BLToolkit.EditableObjects;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Holder;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Интерфейс парсера данных заявки
    /// </summary>
    public interface ITaskParser
    {
        LicenseHolder ParseHolder(Task task);

        HolderInfo ParseHolderInfo(Task task);

        License ParseLicense(Task task);

        LicenseInfo ParseLicenseInfo(Task task);

        EditableList<LicenseObject> ParseLicenseObjects(Task task);
    }
}