using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class PostfixOperatorExpressionEater : ExpressionEater<IPostfixOperatorExpression>
    {
        public PostfixOperatorExpressionEater(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IPostfixOperatorExpression expression)
        {
            return Eater.Eat(snapshot, expression.Operand);
        }
    }
}
