using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Supervision.SupervisionException
{
    /// <summary>
    /// Класс исключений для обработки ошибок "Нет следующего этапа ведения тома. Текущий этап ведения является последним."
    /// </summary>
    public class NoNextScenarioStepException : BLLException
    {
        public NoNextScenarioStepException()
            : base("Нет следующего этапа ведения тома. Текущий этап ведения является последним.")
        {
            
        }
    }
}