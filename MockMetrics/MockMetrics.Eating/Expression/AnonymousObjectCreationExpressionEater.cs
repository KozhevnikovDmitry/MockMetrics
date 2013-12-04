using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AnonymousObjectCreationExpressionEater : ExpressionEater<IAnonymousObjectCreationExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public AnonymousObjectCreationExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IAnonymousObjectCreationExpression expression)
        {
            // TODO: cover by functional tests
            foreach (var memberDeclaration in expression.AnonymousInitializer.MemberInitializers)
            {
                var memberMetrics = Eater.Eat(snapshot, memberDeclaration.Expression);
                memberMetrics.Scope = Scope.Local;
                snapshot.AddOperand(memberDeclaration, memberMetrics);
            }

            return Metrics.Create(Variable.Data);
        }
    }
}
