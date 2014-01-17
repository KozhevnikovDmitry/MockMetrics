using PostGrad.Core.Common;

namespace PostGrad.Core.BL
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
}