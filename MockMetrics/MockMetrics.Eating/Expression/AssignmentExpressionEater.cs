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
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression.Dest as IReferenceExpression);
                if (declaredElement is IVariableDeclaration)
                { 
                    // TODO : check on properties, fields, events, parameters
                    if (declaredElement is IEventDeclaration)
                    {
                        return Eater.Eat(snapshot, expression.Dest);
                    }

                    return EatVariableDeclaration(snapshot, declaredElement as IVariableDeclaration, sourceMetrics);
                }
            }

            throw new UnexpectedAssignDestinationException(expression.Dest, this, expression);
        }

        private Metrics EatVariableDeclaration(ISnapshot snapshot, IVariableDeclaration variableDeclaration, Metrics sourceMetrics)
        {
            var assigneeMetrics = _metricHelper.MetricsForAssignee(sourceMetrics);

            if (variableDeclaration is ILocalVariableDeclaration)
            {
                if ((variableDeclaration as ILocalVariableDeclaration).Initial == null)
                {
                    snapshot.Except(variableDeclaration);
                }
            }

            if (variableDeclaration is IUnsafeCodeFixedPointerDeclaration)
            {
                if ((variableDeclaration as IUnsafeCodeFixedPointerDeclaration).Initial == null)
                {
                    snapshot.Except(variableDeclaration);
                }
            }

            snapshot.AddVariable(variableDeclaration, assigneeMetrics);
            return assigneeMetrics;
        }
    }
}
