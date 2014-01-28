using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.InspectException
{
    /// <summary>
    /// Класс исключений для ошибок "Попытка заведения более одной выездной проверки на этап ведения тома"
    /// </summary>
    public class PrepareMoreThanOneInspectionException : BLLException
    {
        public PrepareMoreThanOneInspectionException()
            : base("Попытка заведения более одной выездной проверки на этап ведения тома")
        {
            
        }
    }
}