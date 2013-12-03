using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class BinaryExpressionEater : ExpressionEater<IBinaryExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public BinaryExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IBinaryExpression expression)
        {
            var leftMetrics = Eater.Eat(snapshot, expression.LeftOperand);
            var rightMetrics = Eater.Eat(snapshot, expression.RightOperand);

            return _metricHelper.VarTypeMerge(leftMetrics, rightMetrics);
        }
    }
}
