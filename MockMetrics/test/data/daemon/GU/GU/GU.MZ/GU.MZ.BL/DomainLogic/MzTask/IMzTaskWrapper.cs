namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// Интерфейс MZ-обёртки заявки
    /// </summary>
    public interface IMzTaskWrapper
    {
        /// <summary>
        /// Тип услуги лицензирования по виду лицензионной деятельности
        /// </summary>
        LicenseServiceType LicenseServiceType { get; }

        /// <summary>
        /// Тип услуги лицензирования по типу действия над лицензией
        /// </summary>
        LicenseActionType LicenseActionType { get; }
    }
}