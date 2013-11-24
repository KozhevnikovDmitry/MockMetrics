using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class PrefixOperatorExpressionEater : ExpressionEater<IPrefixOperatorExpression>
    {
        public PrefixOperatorExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IPrefixOperatorExpression expression, bool innerEat)
        {
            return Eater.Eat(snapshot, expression.Operand, innerEat);
        }
    }
}