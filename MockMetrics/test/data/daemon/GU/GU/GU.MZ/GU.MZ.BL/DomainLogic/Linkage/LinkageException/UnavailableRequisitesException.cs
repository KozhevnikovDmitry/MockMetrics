using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    public class UnavailableRequisitesException : BLLException
    {
        public UnavailableRequisitesException()
            : base("Попытка привязки тома к недоступным реквизитам лицензиата")
        {

        }
    }
}