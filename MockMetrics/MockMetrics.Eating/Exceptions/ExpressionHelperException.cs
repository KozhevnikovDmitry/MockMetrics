using System;

namespace MockMetrics.Eating.Exceptions
{
    public class ExpressionHelperException : ApplicationException
    {
        public string Message { get; private set; }

        public object Source { get; private set; }

        public ExpressionHelperException(string message, object source)
        {
            Message = message;
            Source = source;
        }
    }
}
