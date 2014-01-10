using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AsExpressionEater : ExpressionEater<IAsExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public AsExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IAsExpression expression)
        {
            snapshot.AddVariable(expression.TypeOperand, Variable.Library);

            var operandMetrics = Eater.Eat(snapshot, expression.Operand);

            return _metricHelper.MetricsForCasted(snapshot, operandMetrics, expression.TypeOperand);
        }
    }
}
