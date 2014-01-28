using System;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    /// <summary>
    /// Класс исключение для ошибок "Сценарий ведения тома не сформирован"
    /// </summary>
    public class WrongScenarioSteplistException : Exception
    {
        public WrongScenarioSteplistException()
            :base("Сценарий ведения тома не сформирован.")
        {
            
        }
    }
}