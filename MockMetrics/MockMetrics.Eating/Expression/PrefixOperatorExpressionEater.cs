using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class PrefixOperatorExpressionEater : ExpressionEater<IPrefixOperatorExpression>
    {
        public PrefixOperatorExpressionEater(IEater eater) : base(eater)
        {
        }

        public override Metrics Eat(ISnapshot snapshot, IPrefixOperatorExpression expression)
        {
            return Eater.Eat(snapshot, expression.Operand);
        }
    }
}