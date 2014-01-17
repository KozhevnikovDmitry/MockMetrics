using System;

namespace PostGrad.Core.BL
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