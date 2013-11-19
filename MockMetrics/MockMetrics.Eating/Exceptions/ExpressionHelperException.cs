using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Exceptions
{
    public class ExpressionHelperException : ApplicationException
    {
        public string Message { get; private set; }

        public ICSharpTreeNode Source { get; private set; }

        public ExpressionHelperException(string message, ICSharpTreeNode source)
        {
            Message = message;
            Source = source;
        }
    }
}
