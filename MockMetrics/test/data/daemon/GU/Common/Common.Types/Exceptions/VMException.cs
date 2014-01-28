using System;

namespace Common.Types.Exceptions
{
    /// <summary>
    ///  Класс исключений базовый для всех исключений в возникающих во ViewModel классах.
    /// </summary>
    public class VMException : GUException
    {
        public VMException()
            : base()
        {
        }

        public VMException(string message)
            : base(message)
        {
        }


        public VMException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public VMException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
