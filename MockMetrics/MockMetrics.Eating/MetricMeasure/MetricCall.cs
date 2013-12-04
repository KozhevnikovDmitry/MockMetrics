using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public class MetricCall : IMetricCall
    {
        public MetricCall(IInvocationExpression invocationExpression, Scope scope, Call call)
        {
            Node = invocationExpression;
            Scope = scope;
            Call = call;
        }

        public MetricCall(ITypeofExpression typeofElement, Scope scope, Call call)
        {
            Node = typeofElement;
            Scope = scope;
            Call = call;
        }

        public ICSharpTreeNode Node { get; private set; }
        public int Depth { get; private set; }
        public Scope Scope { get; private set; }
        public Call Call { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}, Scope=[{1}]; Call=[{2}];", Node, Scope, Call);
        }
    }
}