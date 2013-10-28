using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class InvocationEater : IExpressionEater<IInvocationExpression>
    {
        public ExpressionKind Eat(Snapshot snapshot, IMethodDeclaration unitTest, IInvocationExpression expression)
        {
            foreach (ICSharpArgument arg in expression.Arguments)
            {
                ExpressionKind kind = Eater.Eat(snapshot, unitTest, arg.Value);
                snapshot.AddTreeNode(kind, arg);
            }

            IDeclaredElement invokedMethod =
                expression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;

            if (invokedMethod.ToString().StartsWith("Method:Moq.Mock.Of()"))
            {
                return ExpressionKind.Stub;
            }

            if (invokedMethod.ToString().StartsWith("Method:NUnit.Framework.Assert"))
            {
                return ExpressionKind.Assert;
            }

            if (invokedMethod.ToString().StartsWith("Method:Moq.Mock.Verify"))
            {
                return ExpressionKind.Assert;
            }

            return ExpressionKind.TargetCall;
        }
    }
}