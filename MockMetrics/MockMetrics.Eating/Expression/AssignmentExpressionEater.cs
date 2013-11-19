using System;
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

        public override ExpressionKind Eat(ISnapshot snapshot, IAssignmentExpression expression)
        {
            var sourceKind = Eater.Eat(snapshot, expression.Source);

            if (expression.Dest is IReferenceExpression)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression.Dest as IReferenceExpression);

                if (declaredElement is IEvent )
                {
                    return ExpressionKind.None;
                }

                if (declaredElement is IVariableDeclaration && !(declaredElement is ILocalVariableDeclaration))
                {
                    return ExpressionKind.None;
                }

                var assignmentKind = _expressionKindHelper.KindOfAssignment(sourceKind);
                snapshot.AddTreeNode(assignmentKind, expression.Dest);

                return assignmentKind;
            }

            throw new UnexpectedAssignDestinationException(expression.Dest, this, expression);
        }
    }
}
