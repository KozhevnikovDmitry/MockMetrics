using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Impl.reflection2.elements.Context;

namespace MockMetrics.Eating.Expression
{
    public class InvocationEater : ExpressionEater<IInvocationExpression>
    {
        public InvocationEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            foreach (ICSharpArgument arg in expression.Arguments)
            {
                ExpressionKind kind = Eater.Eat(snapshot, arg.Value);
                snapshot.AddTreeNode(kind, arg);
            }
            
            IDeclaredElement invoked =
                expression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;

            if (invoked.ToString().StartsWith("Method:Moq.Mock.Of()"))
            {
                return ExpressionKind.Stub;
            }

            if (invoked.ToString().StartsWith("Method:NUnit.Framework.Assert"))
            {
                return ExpressionKind.Assert;
            }

            if (invoked.ToString().StartsWith("Method:Moq.Mock.Verify"))
            {
                return ExpressionKind.Assert;
            }

            if (invoked is IMethod)
            {
                var invokedMethod = invoked as IMethod;
                if (snapshot.IsInTestScope(invokedMethod.Module.Name))
                {
                    snapshot.AddTreeNode(ExpressionKind.TargetCall, expression);
                    return ExpressionKind.TargetCall;
                }
                else
                {
                    return ExpressionKind.Stub;
                }
            }

            return ExpressionKind.Stub;

        }
    }
}