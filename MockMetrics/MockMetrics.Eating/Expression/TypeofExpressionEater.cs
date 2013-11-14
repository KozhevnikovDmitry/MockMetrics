using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class TypeofExpressionEater : ExpressionEater<ITypeofExpression>
    {
        public TypeofExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, ITypeofExpression expression)
        {
            return ExpressionKind.StubCandidate;
        }
    }
}