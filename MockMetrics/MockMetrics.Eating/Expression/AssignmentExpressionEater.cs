using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class AssignmentExpressionEater : ExpressionEater<IAssignmentExpression>
    {
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly ExpressionKindHelper _expressionKindHelper;

        public AssignmentExpressionEater(IEater eater, EatExpressionHelper eatExpressionHelper, ExpressionKindHelper expressionKindHelper)
            : base(eater)
        {
            _eatExpressionHelper = eatExpressionHelper;
            _expressionKindHelper = expressionKindHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IAssignmentExpression expression, bool innerEat)
        {
            var sourceKind = Eater.Eat(snapshot, expression.Source, innerEat);

            if (expression.Dest is IReferenceExpression)
            { 
                var assignmentKind = _expressionKindHelper.KindOfAssignment(sourceKind);
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression.Dest as IReferenceExpression);
                if (declaredElement is IVariableDeclaration)
                {
                    return EatVariableDeclaration(snapshot, declaredElement as IVariableDeclaration, assignmentKind);
                }
            }

            throw new UnexpectedAssignDestinationException(expression.Dest, this, expression);
        }

        private ExpressionKind EatVariableDeclaration(ISnapshot snapshot, IVariableDeclaration variableDeclaration, ExpressionKind assignmentKind)
        {
            // TODO : check on properties, fields, events, parameters
            if (variableDeclaration is IEventDeclaration)
            {
                return ExpressionKind.None;
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

            snapshot.Add(assignmentKind, variableDeclaration);
            return assignmentKind;
        }
    }
}
