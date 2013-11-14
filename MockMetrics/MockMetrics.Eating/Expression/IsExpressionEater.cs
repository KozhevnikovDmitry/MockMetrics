using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class IsExpressionEater : ExpressionEater<IIsExpression>
    {
        public IsExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IIsExpression expression)
        {
            var kind = Eater.Eat(snapshot, expression.Operand);
            snapshot.AddTreeNode(kind, expression.Operand);

            return ExpressionKind.StubCandidate;
        }
    }
}
