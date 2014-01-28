using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключений дял обработки ошибок "Неправильный тип сценария для процедуры привязки".
    /// </summary>
    public class WrongSceanrioTypeForLinkagerException : BLLException
    {
        public WrongSceanrioTypeForLinkagerException()
            : base("Неправильный тип сценария для процедуры привязки")
        {
            
        }
    }
}