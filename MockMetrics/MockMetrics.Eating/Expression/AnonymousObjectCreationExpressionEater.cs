using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AnonymousObjectCreationExpressionEater : ExpressionEater<IAnonymousObjectCreationExpression>
    {
        public AnonymousObjectCreationExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IAnonymousObjectCreationExpression expression)
        {
            // TODO: cover by functional tests
            foreach (var memberDeclaration in expression.AnonymousInitializer.MemberInitializers)
            {
                var varType = Eater.Eat(snapshot, memberDeclaration.Expression);
                snapshot.AddVariable(memberDeclaration, Scope.Local, , varType);
            }

            return VarType.Library;
        }
    }
}
