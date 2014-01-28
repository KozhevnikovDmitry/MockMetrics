using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.GrantResult.GrantResultException
{
    /// <summary>
    /// Класс исключений для ошибок "Попытка работы с результатом тома по заявкой в неправильном статусе"
    /// </summary>
    public class GranterWorksWithTaskWithWrongStatusException : BLLException
    {
        public GranterWorksWithTaskWithWrongStatusException()
            : base("Попытка работы с результатом тома по заявкой в неправильном статусе")
        {
            
        }
    }
}