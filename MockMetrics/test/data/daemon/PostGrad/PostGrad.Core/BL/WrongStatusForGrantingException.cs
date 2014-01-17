using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    public class WrongStatusForGrantingException : BLLException
    {
        public WrongStatusForGrantingException()
            : base("Попытка работы с результатом тома по заявкой в неправильном статусе")
        {
            
        }
    }
}