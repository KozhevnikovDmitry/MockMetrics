using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.InspectException
{
    /// <summary>
    /// Класс исключения для ошибок "Попытка доступа к проверке, которая ещё не создана"
    /// </summary>
    public class AccessBeforePrepareInspectionException : BLLException
    {
        public AccessBeforePrepareInspectionException()
            : base("Попытка доступа к проверке, которая ещё не создана")
        {
            
        }
    }
}