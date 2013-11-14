using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;

        public InvocationExpressionEater(IEater eater, EatExpressionHelper expressionHelper)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            foreach (ICSharpArgument arg in expression.Arguments)
            {
                ExpressionKind kind = Eater.Eat(snapshot, arg.Value);
                if (kind != ExpressionKind.StubCandidate)
                {
                    snapshot.AddTreeNode(kind, arg);
                }
            }

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

            return ExpressionKind.StubCandidate;

        }
    }
}