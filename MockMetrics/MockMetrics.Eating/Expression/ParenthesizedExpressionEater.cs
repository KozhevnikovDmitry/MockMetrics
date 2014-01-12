using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ParenthesizedExpressionEater : ExpressionEater<IParenthesizedExpression>
    {
        public ParenthesizedExpressionEater(IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IParenthesizedExpression expression)
        {
            return Eater.Eat(snapshot, expression.Expression);
        }
    }
}