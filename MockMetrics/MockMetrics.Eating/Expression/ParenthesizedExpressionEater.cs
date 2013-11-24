using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ParenthesizedExpressionEater : ExpressionEater<IParenthesizedExpression>
    {
        public ParenthesizedExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IParenthesizedExpression expression, bool innerEat)
        {
            return Eater.Eat(snapshot, expression.Expression, innerEat);
        }
    }
}