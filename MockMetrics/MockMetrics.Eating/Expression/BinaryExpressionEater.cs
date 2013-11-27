using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class BinaryExpressionEater : ExpressionEater<IBinaryExpression>
    {
        private readonly MetricHelper _metricHelper;

        public BinaryExpressionEater(IEater eater, MetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override VarType Eat(ISnapshot snapshot, IBinaryExpression expression)
        {
            var leftVarType = Eater.Eat(snapshot, expression.LeftOperand);
            var rightVarType = Eater.Eat(snapshot, expression.RightOperand);

            return _metricHelper.CastExpressionType(leftVarType, rightVarType);
        }
    }
}
