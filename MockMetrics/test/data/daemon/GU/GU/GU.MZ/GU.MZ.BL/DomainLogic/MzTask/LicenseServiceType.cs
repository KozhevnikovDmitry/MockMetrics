using System.ComponentModel;

namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// Тип услуги лицензирования по виду лицензионной деятельности
    /// </summary>
    public enum LicenseServiceType
    {
        [Description("Нарк")]
        Drug,
        [Description("Мед")]
        Med,
        [Description("Фарм")]
        Farm
    }
}