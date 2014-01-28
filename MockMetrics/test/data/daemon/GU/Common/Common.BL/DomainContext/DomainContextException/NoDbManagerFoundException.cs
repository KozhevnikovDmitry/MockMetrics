using Common.Types.Exceptions;

namespace Common.BL.DomainContext.DomainContextException
{
    /// <summary>
    /// Класс исключение для ошибки "Не обнаружено ни одно менеджера базы данных для доменного контекста."
    /// </summary>
    public class NoDbManagerFoundException : BLLException
    {
        public NoDbManagerFoundException()
            : base("Не обнаружено ни одно менеджера базы данных для доменного контекста.")
        {

        }
    }
}