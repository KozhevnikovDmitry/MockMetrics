using System.Collections.Generic;
using BLToolkit.EditableObjects;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.Licensing.Renewal;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Интерфейс парсера данных заявки
    /// </summary>
    public interface IParser
    {
        LicenseHolder ParseHolder(Task task);

        HolderInfo ParseHolderInfo(Task task);

        License ParseLicense(Task task);

        LicenseInfo ParseLicenseInfo(Task task);

        EditableList<LicenseObject> ParseLicenseObjects(Task task);

        Dictionary<ContentNode, RenewalType> ParseRenewalData(Task task);

        EditableList<LicenseObject> ParseRenewalLicenseObjects(ContentNode renewalNode);
    }
}