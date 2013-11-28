using JetBrains.ReSharper.Psi.CSharp.Tree;
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
                var assignmentKind = _metricHelper.MetricsOfAssignment(sourceMetrics);
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression.Dest as IReferenceExpression);
                if (declaredElement is IVariableDeclaration)
                {
                    return EatVariableDeclaration(snapshot, declaredElement as IVariableDeclaration, assignmentKind);
                }
            }

            throw new UnexpectedAssignDestinationException(expression.Dest, this, expression);
        }

        private VarType EatVariableDeclaration(ISnapshot snapshot, IVariableDeclaration variableDeclaration, VarType assignmentType)
        {
            // TODO : check on properties, fields, events, parameters
            if (variableDeclaration is IEventDeclaration)
            {
                return VarType.None;
            }

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

            snapshot.AddVariable(variableDeclaration, , , assignmentType);
            return assignmentType;
        }
    }
}
