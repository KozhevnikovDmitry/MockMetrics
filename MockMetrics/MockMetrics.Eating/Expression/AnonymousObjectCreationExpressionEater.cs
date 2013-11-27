using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AnonymousObjectCreationExpressionEater : ExpressionEater<IAnonymousObjectCreationExpression>
    {
        private readonly MetricHelper _metricHelper;

        public AnonymousObjectCreationExpressionEater(IEater eater, MetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override VarType Eat(ISnapshot snapshot, IAnonymousObjectCreationExpression expression)
        {
            // TODO: cover by functional tests
            foreach (var memberDeclaration in expression.AnonymousInitializer.MemberInitializers)
            {
                var varType = Eater.Eat(snapshot, memberDeclaration.Expression);
                snapshot.AddOperand(memberDeclaration, Scope.Local, Aim.Data, varType);
            }

            return VarType.Internal;
        }
    }
}
