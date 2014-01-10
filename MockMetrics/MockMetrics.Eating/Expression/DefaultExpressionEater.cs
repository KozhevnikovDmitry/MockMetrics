using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class DefaultExpressionEater : ExpressionEater<IDefaultExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public DefaultExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IDefaultExpression expression)
        {
            return _metricHelper.MetricsForType(snapshot, expression.TypeName);
        }
    }
}
