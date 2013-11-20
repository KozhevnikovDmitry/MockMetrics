using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class UnaryOperatorExpressionEater : ExpressionEater<IUnaryOperatorExpression>
    {
        public UnaryOperatorExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IUnaryOperatorExpression expression)
        {
            return Eater.Eat(snapshot, expression.Operand);
        }
    }
}