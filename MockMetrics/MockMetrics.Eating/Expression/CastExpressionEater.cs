using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class CastExpressionEater : ExpressionEater<ICastExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public CastExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, ICastExpression expression)
        {
            snapshot.AddOperand(expression.TargetType, Metrics.Create(Scope.Local, Variable.Data));
            var operandMetrics = Eater.Eat(snapshot, expression.Op);
            return _metricHelper.MetricsForCasted(snapshot, operandMetrics, expression.TargetType);
        }
    }
}