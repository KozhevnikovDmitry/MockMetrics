using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public class MetricCall : IMetricCall
    {
        public MetricCall(IInvocationExpression invocation, Scope scope, Call call)
        {
            Node = invocation;
            Scope = scope;
            Call = call;
        }

        public MetricCall(ITypeofExpression invocation, Scope scope, Call call)
        {
            Node = invocation;
            Scope = scope;
            Call = call;
        }

        public ICSharpTreeNode Node { get; private set; }
        public int Depth { get; private set; }
        public Scope Scope { get; private set; }
        public Call Call { get; private set; }
    }
}