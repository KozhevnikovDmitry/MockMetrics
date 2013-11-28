using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class AnonymousMethodParameterDeclarationEater : VariableDeclarationEater<IAnonymousMethodParameterDeclaration>
    {
        private readonly IMetricHelper _metricHelper;

        public AnonymousMethodParameterDeclarationEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IAnonymousMethodParameterDeclaration variableDeclaration)
        {
            var metrics = _metricHelper.MetricsForType(snapshot, variableDeclaration.Type);
            metrics.Scope = Scope.Local;
            snapshot.AddVariable(variableDeclaration, metrics);
            return metrics;
        }
    }
}