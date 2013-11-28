using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AssignmentExpressionEater : ExpressionEater<IAssignmentExpression>
    {
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly IMetricHelper _metricHelper;

        public AssignmentExpressionEater(IEater eater, EatExpressionHelper eatExpressionHelper, IMetricHelper metricHelper)
            : base(eater)
        {
            _eatExpressionHelper = eatExpressionHelper;
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IAssignmentExpression expression)
        {
            var sourceMetrics = Eater.Eat(snapshot, expression.Source);

            if (expression.Dest is IReferenceExpression)
            {
                var destMetrics = Eater.Eat(snapshot, expression.Dest);
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression.Dest as IReferenceExpression);
                if (declaredElement is IVariableDeclaration)
                { 
                    // TODO : check on properties, fields, events, parameters
                    if (declaredElement is IEventDeclaration)
                    {
                        return destMetrics;
                    }

                    var assigneeMetrics = _metricHelper.MetricsMerge(destMetrics, sourceMetrics);
                    snapshot.AddVariable(declaredElement as IVariableDeclaration, assigneeMetrics);
                    return assigneeMetrics;
                }
            }

            throw new UnexpectedAssignDestinationException(expression.Dest, this, expression);
        }
    }
}
