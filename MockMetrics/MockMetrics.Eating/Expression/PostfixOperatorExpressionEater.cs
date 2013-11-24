using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class PostfixOperatorExpressionEater : ExpressionEater<IPostfixOperatorExpression>
    {
        public PostfixOperatorExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IPostfixOperatorExpression expression, bool innerEat)
        {
            return Eater.Eat(snapshot, expression.Operand, innerEat);
        }
    }
}
