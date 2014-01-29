using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AssignmentExpressionEater : ExpressionEater<IAssignmentExpression>
    {
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public AssignmentExpressionEater(IEater eater, IMetricHelper metricHelper, EatExpressionHelper eatExpressionHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IAssignmentExpression expression)
        {
            var sourceMetrics = Eater.Eat(snapshot, expression.Source);
            var destMetrics = Eater.Eat(snapshot, expression.Dest);

            if (expression.Dest is IReferenceExpression)
            {
                var assigneeMetrics = _metricHelper.MetricsMerge(destMetrics, sourceMetrics);
                var dest = expression.Dest as IReferenceExpression;
                var declaration = _eatExpressionHelper.GetReferenceDeclaration(dest);
                if (declaration is NullCsharpDeclaration)
                {
                    snapshot.AddVariable(dest, assigneeMetrics);
                }
                else
                {
                    snapshot.AddVariable(declaration, assigneeMetrics);
                }
            }
            else
            {
                Eater.Eat(snapshot, expression.Dest);
            }

            return Variable.None;
        }
    }
}
