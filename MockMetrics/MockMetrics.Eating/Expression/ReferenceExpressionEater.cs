using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        public ReferenceExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            return ExpressionKind.None;
        }
    }
}