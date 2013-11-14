using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating
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