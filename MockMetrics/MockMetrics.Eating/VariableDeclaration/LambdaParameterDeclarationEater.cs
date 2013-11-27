using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LambdaParameterDeclarationEater : VariableDeclarationEater<ILambdaParameterDeclaration>
    {
        private readonly MetricHelper _metricHelper;

        public LambdaParameterDeclarationEater(IEater eater, MetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, ILambdaParameterDeclaration variableDeclaration)
        {
            var metrics = _metricHelper.MetricsForType(snapshot, variableDeclaration.Type);
            metrics.Scope = Scope.Local;
            snapshot.AddVariable(variableDeclaration, metrics);
            return metrics;
        }
    }
}