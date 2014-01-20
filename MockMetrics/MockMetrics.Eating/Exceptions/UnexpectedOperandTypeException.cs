using System;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Exceptions
{
    public class UnexpectedOperandTypeException : ApplicationException
    {
        public Snapshot Snapshot { get; private set; }
        public object Node { get; private set; }

        public UnexpectedOperandTypeException(Snapshot snapshot, object node)
            : base("Unexpected operand type in snapshot")
        {
            Snapshot = snapshot;
            Node = node;
        }
    }
}