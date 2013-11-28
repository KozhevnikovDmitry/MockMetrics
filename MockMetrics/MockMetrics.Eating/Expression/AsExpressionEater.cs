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

        public override Metrics Eat(ISnapshot snapshot, IAsExpression expression)
        {
            snapshot.AddOperand(expression.TypeOperand, Metrics.Create(Scope.Local, VarType.Library, Aim.Data));

            var operandMetrics = Eater.Eat(snapshot, expression.Operand);

            return _metricHelper.MetricsForCasted(snapshot, operandMetrics, expression.TypeOperand);
        }
    }
}
