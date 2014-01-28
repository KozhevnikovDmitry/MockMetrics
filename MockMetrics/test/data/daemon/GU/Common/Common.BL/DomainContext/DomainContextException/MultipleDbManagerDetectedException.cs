using Common.Types.Exceptions;

namespace Common.BL.DomainContext.DomainContextException
{
    /// <summary>
    /// Класс исключение для ошибки "Обнаружено больше одного менеджера базы данных для доменного контекста.".
    /// </summary>
    public class MultipleDbManagerDetectedException : BLLException
    {
        public MultipleDbManagerDetectedException()
            : base("Обнаружено больше одного менеджера базы данных для доменного контекста.")
        {

        }
    }
}
