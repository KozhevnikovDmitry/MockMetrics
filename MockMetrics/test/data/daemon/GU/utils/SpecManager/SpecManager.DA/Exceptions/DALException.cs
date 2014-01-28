using System;

using SpecManager.BL.Interface;

namespace SpecManager.DA.Exceptions
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

        public DALException(Exception exception)
            : base("DALException", exception)
        {
        }
    }
}
