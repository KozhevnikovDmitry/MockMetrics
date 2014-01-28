using Common.Types.Exceptions;

namespace Common.DA.DAException
{
    /// <summary>
    /// Класс исключений для ошибки "Транзакция уже была зафиксирована или отменена.".
    /// </summary>
    public class TransactionControlException : DALException
    {
        public TransactionControlException()
            : base("Транзакция уже была зафиксирована или отменена.")
        {
            
        }
    }
}
