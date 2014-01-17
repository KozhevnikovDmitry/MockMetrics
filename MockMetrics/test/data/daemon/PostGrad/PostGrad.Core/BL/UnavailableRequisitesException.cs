using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    public class UnavailableRequisitesException : BLLException
    {
        public UnavailableRequisitesException()
            : base("Попытка привязки тома к недоступным реквизитам лицензиата")
        {

        }
    }
}