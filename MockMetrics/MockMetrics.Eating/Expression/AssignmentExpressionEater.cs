using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AssignmentExpressionEater : ExpressionEater<IAssignmentExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public AssignmentExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IAssignmentExpression expression)
        {
            var sourceMetrics = Eater.Eat(snapshot, expression.Source);
            var destMetrics = Eater.Eat(snapshot, expression.Dest);

            if (expression.Dest is IReferenceExpression)
            {
                var assigneeMetrics = _metricHelper.MetricsMerge(destMetrics, sourceMetrics);;
                snapshot.AddVariable(expression.Dest as IReferenceExpression, assigneeMetrics);
            }
            else
            {
                Eater.Eat(snapshot, expression.Dest);
            }

            return Variable.None;
        }
    }
}
