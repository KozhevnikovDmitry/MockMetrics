using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Exceptions
{
    public class OperandNotFoundInSnapshotException : ApplicationException
    {
        public ISnapshot Snapshot { get; private set; }
        public ICSharpTreeNode Operand { get; private set; }

        public OperandNotFoundInSnapshotException(ISnapshot snapshot, ICSharpTreeNode operand)
            :base("Operand is not found in shanpshot")
        {
            Snapshot = snapshot;
            Operand = operand;
        }
    }
}