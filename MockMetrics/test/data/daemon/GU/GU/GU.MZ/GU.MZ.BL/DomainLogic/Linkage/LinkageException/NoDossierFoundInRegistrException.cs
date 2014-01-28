using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключение для ошибки "Лицензионного дела для лицензиата по виду деятельности не найдено в реестре лицензионных дел."
    /// </summary>
    public class NoDossierFoundInRegistrException : BLLException
    {
        public NoDossierFoundInRegistrException(int licenseHolderId, int licensedActivityId)
            : base(
                string.Format(
                    "Лицензионного дела для лицензиата № {0} по виду деятельности {1} не найдено в реестре лицензионных дел.",
                    licenseHolderId,
                    licensedActivityId))
        {

        }
    }
}