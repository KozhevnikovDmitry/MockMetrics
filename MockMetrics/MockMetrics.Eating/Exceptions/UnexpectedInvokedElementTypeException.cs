using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Exceptions
{
    public class UnexpectedInvokedElementTypeException : EatingException
    {
        public IDeclaredElement DeclaredElement { get; private set; }

        public UnexpectedInvokedElementTypeException(IDeclaredElement declaredElement, ICSharpNodeEater eater, ICSharpTreeNode node)
            : base(string.Format("Unexpected reference declared type. Expected field, property, event, constant, variable or class, but was [{0}]", declaredElement.GetType()), eater, node)
        {
            DeclaredElement = declaredElement;
        }
    }
}