using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключение для ошибки "Попытка добавления дела в реестр без привязки к лицензиату".
    /// </summary>
    public class NoBoundedLicenseHolderException : GUException
    {
        public NoBoundedLicenseHolderException()
            : base("Попытка добавления дела в реестр без привязки к лицензиату")
        {
            
        }
    }
}