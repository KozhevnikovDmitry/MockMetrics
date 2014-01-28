using System;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Exceptions
{
    /// <summary>
    ///  Класс исключений базовый для всех исключений в слое бизнес логики всех предметных областей.
    /// </summary>
    public class BLLException : GUException
    {
        public BLLException()
            : base()
        {
        }

        public BLLException(string message)
            : base(message)
        {
        }


        public BLLException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public BLLException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
