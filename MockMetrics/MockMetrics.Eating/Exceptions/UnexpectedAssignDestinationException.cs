using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.Exceptions
{
    public class UnexpectedAssignDestinationException : EatingException
    {
        public ICSharpExpression Destination { get; private set; }

        public UnexpectedAssignDestinationException(ICSharpExpression destination, AssignmentExpressionEater eater, ICSharpTreeNode node) 
            : base(string.Format("Unexpected assign destination. Expected IReferenceExpression, but was [{0}]", destination.GetType()), eater, node)
        {
            Destination = destination;
        }
    }
}