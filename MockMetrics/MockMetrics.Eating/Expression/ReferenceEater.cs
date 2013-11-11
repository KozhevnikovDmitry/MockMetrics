using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceEater : ExpressionEater<IReferenceExpression>
    {
        public ReferenceEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            return ExpressionKind.None;
        }
    }
}