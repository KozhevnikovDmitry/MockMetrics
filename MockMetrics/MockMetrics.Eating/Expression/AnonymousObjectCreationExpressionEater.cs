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

        public override Metrics Eat(ISnapshot snapshot, IAnonymousObjectCreationExpression expression)
        {
            // TODO: cover by functional tests
            foreach (var memberDeclaration in expression.AnonymousInitializer.MemberInitializers)
            {
                var metrics = Eater.Eat(snapshot, memberDeclaration.Expression);
                var resultMetrics = metrics.AcceptorMetrics();
                resultMetrics.Scope = Scope.Local;
                snapshot.AddOperand(memberDeclaration, resultMetrics);
            }

            return Metrics.Create(VarType.Internal);
        }
    }
}
