using System;

namespace PostGrad.Core.Common
{
    /// <summary>
    /// Класс исключений базовый для всех исключений в проекте гос услуг.
    /// </summary>
    public class BaseException : ApplicationException
    {
        public BaseException()
            : base()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
