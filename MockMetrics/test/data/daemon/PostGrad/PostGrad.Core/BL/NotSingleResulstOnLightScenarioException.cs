using System;
using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Тест на попытку заведения результата при неопределённом результате для облегчённого сценария
    /// </summary>
    public class NotSingleResulstOnLightScenarioException : BLLException
    {
        public NotSingleResulstOnLightScenarioException(InvalidOperationException invalidOperationException)
            : base("Неопределённый результат предоставления услуги для облегчённого сценария", invalidOperationException)
        {
            
        }
    }
}