using JetBrains.ReSharper.Psi;
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
                var assigneeMetrics = _metricHelper.VarTypeMerge(destMetrics, sourceMetrics);
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression.Dest as IReferenceExpression);
                if (declaredElement is IVariableDeclaration)
                { 
                    snapshot.AddVariable(declaredElement as IVariableDeclaration, assigneeMetrics);
                }
                else
                {
                    snapshot.AddOperand(declaredElement as ICSharpTreeNode, assigneeMetrics);
                }
                return assigneeMetrics;
            }

            throw new UnexpectedAssignDestinationException(expression.Dest, this, expression);
        }
    }
}
