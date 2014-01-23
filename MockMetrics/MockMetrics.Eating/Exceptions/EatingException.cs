using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Exceptions
{
    public abstract class EatingException : ApplicationException
    {
        public Exception Exception { get; private set; }

        public string Message { get; private set; }

        public ICSharpNodeEater Eater { get; private set; }

        public ICSharpTreeNode Node { get; private set; }

        public EatingException(string message, ICSharpNodeEater eater, ICSharpTreeNode node)
        {
            Message = message;
            Eater = eater;
            Node = node;
        }

        public EatingException(string message, Exception exception, ICSharpNodeEater eater, ICSharpTreeNode node)
            : this(message, eater, node)
        {
            Exception = exception;
        }

        public EatingException(Exception exception, ICSharpNodeEater eater, ICSharpTreeNode node)
            : this(exception.Message, exception, eater, node)
        {
  
        }

        public override string ToString()
        {
            return InnerToString(this);
        }

        private string InnerToString(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            return string.Format("{0} \n {1}", ex, InnerToString(ex.InnerException));
        }
    }

    public class FailToGetEaterException : EatingException
    {
        public FailToGetEaterException(Exception exception, ICSharpNodeEater eater, ICSharpTreeNode node)
            : base(string.Format("Fail to get eater foк node of type [{0}]", node.GetType()), exception, eater, node)
        {
        }
    }

    public class UnexpextedEatingException : EatingException
    {
        public UnexpextedEatingException(Exception exception, ICSharpNodeEater eater, ICSharpTreeNode node)
            : base("Unexpected exception", exception, eater, node)
        {
        }
    }
}
