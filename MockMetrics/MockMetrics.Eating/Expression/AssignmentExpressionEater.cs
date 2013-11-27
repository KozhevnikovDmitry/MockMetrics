using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AssignmentExpressionEater : ExpressionEater<IAssignmentExpression>
    {
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly VarTypeHelper _varTypeHelper;

        public AssignmentExpressionEater(IEater eater, EatExpressionHelper eatExpressionHelper, VarTypeHelper varTypeHelper)
            : base(eater)
        {
            _eatExpressionHelper = eatExpressionHelper;
            _varTypeHelper = varTypeHelper;
        }

        public override VarType Eat(ISnapshot snapshot, IAssignmentExpression expression)
        {
            var sourceKind = Eater.Eat(snapshot, expression.Source);

            if (expression.Dest is IReferenceExpression)
            { 
                var assignmentKind = _varTypeHelper.KindOfAssignment(sourceKind);
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
