using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class EatExpressionHelper
    {
        public virtual IClass GetCreationClass(IObjectCreationExpression creationExpression)
        {
            return creationExpression.TypeReference.CurrentResolveResult.DeclaredElement as IClass;
        }

        public virtual string GetCreationTypeName(IObjectCreationExpression creationExpression)
        {
            return creationExpression.Type().ToString();
        }

        public virtual IDeclaredElement GetInvokedElement(IInvocationExpression expression)
        {
            return expression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;
        }


        public virtual string GetInvokedElementName(IInvocationExpression expression)
        {
            return expression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement.ToString();
        }
    }
}
