using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class ForeachVariableDeclarationEater : VariableDeclarationEater<IForeachVariableDeclaration>
    {
        private readonly IMetricHelper _metricHelper;

        public ForeachVariableDeclarationEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IForeachVariableDeclaration variableDeclaration)
        {
            var varType = _metricHelper.MetricsForType(snapshot, variableDeclaration.Type);
            snapshot.AddVariable(variableDeclaration, varType);
            return varType;
        }
    }
}