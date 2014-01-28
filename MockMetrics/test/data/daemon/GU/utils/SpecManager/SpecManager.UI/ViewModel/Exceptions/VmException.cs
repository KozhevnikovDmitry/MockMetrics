using System;

using SpecManager.BL.Interface;

namespace SpecManager.UI.ViewModel.Exceptions
{
    public class VmException : GUException
    {
        public VmException()
            : base()
        {
        }

        public VmException(string message)
            : base(message)
        {
        }


        public VmException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public VmException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
