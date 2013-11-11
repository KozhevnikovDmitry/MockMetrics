using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceEater : ExpressionEater<IReferenceExpression>
    {
        public ReferenceEater(Eater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(Snapshot snapshot, IReferenceExpression expression)
        {
            return ExpressionKind.None;
        }
    }
}