using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.ExpertiseException
{
    /// <summary>
    /// Класс исключение для ошибок "Попытка добавить результат документарной проверки до заведения самой проверки"
    /// </summary>
    public class AddResultBeforePrepareExpertiseException : BLLException
    {
        public AddResultBeforePrepareExpertiseException()
            : base("Попытка добавить результат документарной проверки до заведения самой проверки")
        {
            
        }
    }
}