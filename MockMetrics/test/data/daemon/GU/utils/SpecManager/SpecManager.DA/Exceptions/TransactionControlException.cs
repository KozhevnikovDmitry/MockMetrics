using SpecManager.BL.Interface;

namespace SpecManager.DA.Exceptions
{
    /// <summary>
    /// Класс исключений для ошибки "Транзакция уже была зафиксирована или отменена.".
    /// </summary>
    internal class TransactionControlException : GUException
    {
        public TransactionControlException()
            : base("Транзакция уже была зафиксирована или отменена.")
        {
            
        }
    }
}
