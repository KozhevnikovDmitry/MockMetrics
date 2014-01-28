using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.ExpertiseException
{
    /// <summary>
    /// Класс исключений для ошибок "Попытка заведени более одной документарной проверки на этап ведения тома"
    /// </summary>
    public class PrepareMoreThanOneExpertiseException : BLLException
    {
        public PrepareMoreThanOneExpertiseException()
            : base("Попытка заведения более одной документарной проверки на этап ведения тома")
        {
            
        }
    }
}