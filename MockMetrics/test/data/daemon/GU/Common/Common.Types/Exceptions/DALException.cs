using System;

namespace Common.Types.Exceptions
{
    /// <summary>
    ///  Класс исключений базовый для всех исключений в слое доступа к данным.
    /// </summary>
    public class DALException : GUException
    {
        public DALException()
            : base()
        {
        }

        public DALException(string message)
            : base(message)
        {
        }

        public DALException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
