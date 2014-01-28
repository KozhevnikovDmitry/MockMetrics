using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.ExpertiseException
{
    /// <summary>
    /// Класс исключение для ошибок "Попытка заведения результата документарной проверки по недоступному документу"
    /// </summary>
    public class UnavailableDocumentToAddResultException : BLLException
    {
        public UnavailableDocumentToAddResultException()
            :base("Попытка заведения результата документарной проверки по недоступному документу")
        {
            
        }
    }
}