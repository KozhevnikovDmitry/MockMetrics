using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключение для ошибки "Лицензиата с такими ИНН и ОГРН нет в реестре лицензиатов"
    /// </summary>
    public class NoHolderFoundInRegistrException : GUException
    {
        public NoHolderFoundInRegistrException(string inn, string ogrn)
            : base(string.Format("Лицензиата с ИНН={0} и ОГРН={1} нет в реестре лицензиатов", inn, ogrn))
        {
            
        }
    }
}