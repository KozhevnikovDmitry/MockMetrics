using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Класс исключений для ошибок "Попытка работы с результатом тома по заявкой в неправильном статусе"
    /// </summary>
    public class GranterWorksWithTaskWithWrongStatusException : BLLException
    {
        public GranterWorksWithTaskWithWrongStatusException()
            : base("Попытка работы с результатом тома по заявкой в неправильном статусе")
        {
            
        }
    }
}