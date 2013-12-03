using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Exceptions
{
    public class UnexpectedOperandTypeException : ApplicationException
    {
        public Snapshot Snapshot { get; private set; }
        public ICSharpTreeNode Node { get; private set; }

        public UnexpectedOperandTypeException(Snapshot snapshot, ICSharpTreeNode node)
            : base("Unexpected operand type in snapshot")
        {
            Snapshot = snapshot;
            Node = node;
        }
    }
}