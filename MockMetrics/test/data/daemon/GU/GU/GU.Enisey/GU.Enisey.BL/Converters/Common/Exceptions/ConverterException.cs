using System;
using Common.Types.Exceptions;

namespace GU.Enisey.BL.Converters.Common.Exceptions
{
    public class ConverterException : BLLException
    {
        public ConverterException()
            : base()
        {
        }

        public ConverterException(string message)
            : base(message)
        {
        }

        public ConverterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
