using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class AwaitExpressionEater : ExpressionEater<IAwaitExpression>
    {
        public AwaitExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IAwaitExpression expression)
        {
            return Eater.Eat(snapshot, expression.Task);
        }
    }
}