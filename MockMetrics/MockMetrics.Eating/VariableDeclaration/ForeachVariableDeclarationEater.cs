using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class ForeachVariableDeclarationEater : VariableDeclarationEater<IForeachVariableDeclaration>
    {
        private readonly MetricHelper _metricHelper;

        public ForeachVariableDeclarationEater(IEater eater, MetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IForeachVariableDeclaration variableDeclaration)
        {
            var metrics = _metricHelper.MetricsForType(snapshot, variableDeclaration.Type);
            metrics.Scope = Scope.Local;
            snapshot.AddVariable(variableDeclaration, metrics);
            return metrics;
        }
    }
}