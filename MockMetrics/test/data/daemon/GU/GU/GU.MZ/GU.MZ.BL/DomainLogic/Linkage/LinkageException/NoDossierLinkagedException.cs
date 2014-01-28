using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    public class NoDossierLinkagedException : BLLException
    {
        public NoDossierLinkagedException()
            :base("Том не привязан к лицензионному делу, невозможно привязать лицензию")
        {
            
        }
    }

    public class NoHolderLinkagedException : BLLException
    {
        public NoHolderLinkagedException()
            : base("Том не привязан к лицензиату, невозможно привязать лицензию")
        {

        }
    }

    public class NoRequisitesLinkagedException : BLLException
    {
        public NoRequisitesLinkagedException()
            : base("Том не привязан к реквизитам, невозможно привязать лицензию")
        {

        }
    }
}