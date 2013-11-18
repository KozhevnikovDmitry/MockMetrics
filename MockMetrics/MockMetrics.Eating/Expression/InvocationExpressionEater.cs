using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly ExpressionKindHelper _expressionKindHelper;
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;

        public InvocationExpressionEater(IEater eater, 
                                         EatExpressionHelper expressionHelper, 
                                         ExpressionKindHelper expressionKindHelper,
                                         IParentReferenceEater parentReferenceEater,
                                         IArgumentsEater argumentsEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _expressionKindHelper = expressionKindHelper;
            _parentReferenceEater = parentReferenceEater;
            _argumentsEater = argumentsEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            _argumentsEater.Eat(snapshot, expression.Arguments);

            var parentKind = _parentReferenceEater.Eat(snapshot, expression);

            var invokedName = _expressionHelper.GetInvokedElementName(expression);

            if (invokedName.StartsWith("Method:Moq.Mock.Of()"))
            {
                return ExpressionKind.Stub;
            }

            if (invokedName.StartsWith("Method:NUnit.Framework.Assert"))
            {
                return ExpressionKind.Assert;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Verify"))
            {
                return ExpressionKind.Assert;
            }

            var invoked = _expressionHelper.GetInvokedElement(expression);
            if (invoked is IMethod)
            {
                var invokedMethod = invoked as IMethod;
                if (snapshot.IsInTestScope(invokedMethod.Module.Name))
                {
                    snapshot.AddTreeNode(ExpressionKind.TargetCall, expression);
                    return ExpressionKind.TargetCall;
                }
            }

            if (parentKind == ExpressionKind.None)
            {
                return ExpressionKind.StubCandidate;
            }

            var basedOnParentKind = _expressionKindHelper.InvocationKindByParentReferenceKind(parentKind);
            if (basedOnParentKind == ExpressionKind.TargetCall)
            {
                snapshot.AddTreeNode(ExpressionKind.TargetCall, expression);
                return ExpressionKind.TargetCall;
            }

            return basedOnParentKind;
        }
    }
}