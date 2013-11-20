using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class AnonymousObjectCreationExpressionEater : ExpressionEater<IAnonymousObjectCreationExpression>
    {
        public AnonymousObjectCreationExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IAnonymousObjectCreationExpression expression)
        {
            // TODO: cover by functional tests
            foreach (var memberDeclaration in expression.AnonymousInitializer.MemberInitializers)
            {
                var kind = Eater.Eat(snapshot, memberDeclaration.Expression);
                snapshot.Add(kind, memberDeclaration);
            }

            return ExpressionKind.StubCandidate;
        }
    }
}
