using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключение для ошибки "Лицензиат с такими ИНН и\или ОГРН уже заведён в реестре лицензиатов"
    /// </summary>
    public class DuplicateLicenseHolderException : GUException
    {
        public DuplicateLicenseHolderException(string inn, string ogrn)
            : base(string.Format(@"Лицензиат с ИНН={0} и\или ОГРН={1} уже заведён в реестре лицензиатов", inn, ogrn))
        {
            
        }
    }
}