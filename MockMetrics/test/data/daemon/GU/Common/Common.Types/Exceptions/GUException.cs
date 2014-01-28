using System;

namespace Common.Types.Exceptions
{
    /// <summary>
    /// Класс исключений базовый для всех исключений в проекте гос услуг.
    /// </summary>
    public class GUException : ApplicationException
    {
        public GUException()
            : base()
        {
        }

        public GUException(string message)
            : base(message)
        {
        }

        public GUException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
