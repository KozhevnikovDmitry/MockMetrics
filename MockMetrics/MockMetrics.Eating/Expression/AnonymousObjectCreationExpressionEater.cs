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

        public override Variable Eat(ISnapshot snapshot, IAnonymousObjectCreationExpression expression)
        {
            foreach (var memberDeclaration in expression.AnonymousInitializer.MemberInitializers)
            {
                Eater.Eat(snapshot, memberDeclaration);
            }

            return Variable.Library;
        }
    }
}
