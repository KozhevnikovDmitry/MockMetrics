using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class InvocationEater : ExpressionEater<IInvocationExpression>
    {
        public InvocationEater(Eater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(Snapshot snapshot, IInvocationExpression expression)
        {
            foreach (ICSharpArgument arg in expression.Arguments)
            {
                ExpressionKind kind = Eater.Eat(snapshot, arg.Value);
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