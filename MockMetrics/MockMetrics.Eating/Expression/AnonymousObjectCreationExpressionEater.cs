using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AnonymousObjectCreationExpressionEater : ExpressionEater<IAnonymousObjectCreationExpression>
    {
        public AnonymousObjectCreationExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IAnonymousObjectCreationExpression expression)
        {
            // TODO: cover by functional tests
            foreach (var memberDeclaration in expression.AnonymousInitializer.MemberInitializers)
            {
                var memberMetrics = Eater.Eat(snapshot, memberDeclaration.Expression);
                snapshot.AddVariable(memberDeclaration, memberMetrics);
            }

            return Variable.Library;
        }
    }
}
