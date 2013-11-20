using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Exceptions
{
    public class UnexpectedTypeOfNodeToEatException : EatingException
    {
        public UnexpectedTypeOfNodeToEatException(Type expectedType, ICSharpNodeEater eater, ICSharpTreeNode node)
            : base(string.Format("Unexpected type of tree node no eat. Expected [{0}], but was [{1}]", expectedType, node.GetType()), eater, node)
        {
        }

    }
}