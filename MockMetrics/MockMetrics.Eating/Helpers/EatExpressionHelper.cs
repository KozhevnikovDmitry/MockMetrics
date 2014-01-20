using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Helpers
{
    public class EatExpressionHelper
    {
        public virtual IClass GetCreationClass([NotNull] IObjectCreationExpression creationExpression)
        {
            if (creationExpression == null)
                throw new ArgumentNullException("creationExpression");

            if (creationExpression.TypeReference != null)
            {
                if (creationExpression.TypeReference.CurrentResolveResult != null)
                {
                    return creationExpression.TypeReference.CurrentResolveResult.DeclaredElement as IClass;
                }
                else
                {
                    throw new ExpressionHelperException("Null type reference of creation expression", creationExpression);
                }
            }
            else
            {
                throw new ExpressionHelperException("Null resolved result of creation expression", creationExpression);
            }
        }

        public virtual ITypeElement GetUserTypeUsageClass([NotNull] IUserTypeUsage userTypeUsage)
        {
            if (userTypeUsage == null)
                throw new ArgumentNullException("userTypeUsage");

            if (userTypeUsage.ScalarTypeName.Reference.CurrentResolveResult != null)
            {
                return userTypeUsage.ScalarTypeName.Reference.CurrentResolveResult.DeclaredElement as ITypeElement;
            }
            else
            {
                throw new ExpressionHelperException("Null resolved result of type usage", userTypeUsage);
            }
        }

        public virtual ITypeElement GetTypeClass([NotNull] IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var scalarType = type.GetScalarType();
            if (scalarType != null)
            {
                if (scalarType.GetTypeElement() != null)
                {
                    return scalarType.GetTypeElement();
                }
                else
                {
                    throw new ExpressionHelperException("Declared element type is not a class", type);
                }
            }
            else
            {
                throw new ExpressionHelperException("Null scalara type for type", type);
            }
        }

        public virtual string GetCreationTypeName([NotNull] IObjectCreationExpression creationExpression)
        {
            if (creationExpression == null)
                throw new ArgumentNullException("creationExpression");

            return creationExpression.Type().ToString();
        }

        public virtual IDeclaredElement GetInvokedElement([NotNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null)
                throw new ArgumentNullException("invocationExpression");

            if (invocationExpression.InvocationExpressionReference.CurrentResolveResult != null)
            {
                return invocationExpression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;
            }
            else
            {
                throw new ExpressionHelperException("Null resolved result of invocation expression", invocationExpression);
            }
        }

        public virtual IDeclaredElement GetReferenceElement([NotNull] IReferenceExpression referenceExpression)
        {
            if (referenceExpression == null)
                throw new ArgumentNullException("referenceExpression");

            if (referenceExpression.Reference.CurrentResolveResult != null)
            {
                return referenceExpression.Reference.CurrentResolveResult.DeclaredElement;
            }
            else
            {
                throw new ExpressionHelperException("Null resolved result of reference expression", referenceExpression);
            }
        }

        public virtual string GetInvokedElementName([NotNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null)
                throw new ArgumentNullException("invocationExpression");

            if (invocationExpression.InvocationExpressionReference.CurrentResolveResult != null)
            {
                if (invocationExpression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement != null)
                {
                    return invocationExpression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement.ToString();
                }
                else
                {
                    throw new ExpressionHelperException("Null parent reference of invocation expression", invocationExpression);
                }
            }
            else
            {
                throw new ExpressionHelperException("Null resolved result of invocation parent reference expression", invocationExpression);
            }
        }

        public virtual ICSharpExpression GetInvocationReference([NotNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null) 
                throw new ArgumentNullException("invocationExpression");

            if (invocationExpression.ExtensionQualifier == null)
            {
                return null;
            }

            var extensionArgumentInfo =
               invocationExpression.ExtensionQualifier.ManagedConvertible as ExtensionArgumentInfo;

            if (extensionArgumentInfo == null || extensionArgumentInfo.Expression == null)
            {
                return null;
            }

            return extensionArgumentInfo.Expression;
        }

        public virtual bool IsStandaloneExpression([NotNull] IInvocationExpression expression)
        {
            return IsStandaloneExpr(expression);
        }

        public virtual bool IsStandaloneExpression([NotNull] ICSharpLiteralExpression expression)
        {
            return IsStandaloneExpr(expression);
        }

        public virtual bool IsStandaloneExpression([NotNull] IReferenceExpression expression)
        {
            return IsStandaloneExpr(expression);
        }

        private bool IsStandaloneExpr(ICSharpExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (expression.Parent is IExpressionInitializer ||
                expression.Parent is IMemberInitializer ||
                expression.Parent is IAnonymousMemberDeclaration)
            {
                return false;
            }

            if (expression.Parent is IAssignmentExpression &&
               (expression.Parent as IAssignmentExpression).Dest is IReferenceExpression)
            {
                return false;
            }

            return true;
        }

        public virtual bool IsStandaloneObjectCreationExpression([NotNull] IObjectCreationExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (expression.Parent is IExpressionInitializer ||
                expression.Parent is IAssignmentExpression ||
                expression.Parent is IMemberInitializer ||
                expression.Parent is IAnonymousMemberDeclaration)
            {
                return false;
            }

            return true;
        }

        public virtual bool IsReferenceToLocalVariable([NotNull] ISnapshot snapshot,
                                                       [NotNull] IReferenceExpression referenceExpression)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (referenceExpression == null) throw new ArgumentNullException("referenceExpression");
            var declared = GetReferenceElement(referenceExpression);
            if (declared is IVariableDeclaration)
            {
                if (snapshot.Variables.Any(t => t.Node.Equals(declared)))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual IReferenceExpression GetParentReference([NotNull] IReferenceExpression referenceExpression)
        {
            if (referenceExpression == null) throw new ArgumentNullException("referenceExpression");
            return referenceExpression.QualifierExpression as IReferenceExpression;
        }

        public virtual IReferenceExpression GetParentReference([NotNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null) throw new ArgumentNullException("invocationExpression");
            if (invocationExpression.ExtensionQualifier == null)
            {
                return null;
            }

            var mc = invocationExpression.ExtensionQualifier.ManagedConvertible;
            if (mc is ExtensionArgumentInfo)
            {
                return (mc as ExtensionArgumentInfo).Expression as IReferenceExpression;
            }

            return null;
        }

        public virtual bool IsMoqFakeOptionParameter([NotNull] ILambdaParameterDeclaration variableDeclaration)
        {
            if (variableDeclaration == null) throw new ArgumentNullException("variableDeclaration");

            var lambdaExpression = variableDeclaration.Parent.Parent;

            if (lambdaExpression.Parent is ICSharpArgument)
            {
                var arg = lambdaExpression.Parent as ICSharpArgument;
                if (arg.Parent is IArgumentList)
                {
                    var argList = arg.Parent as IArgumentList;

                    if (argList.Parent is IInvocationExpression)
                    {
                        var invocation = argList.Parent as IInvocationExpression;

                        var invokedName = GetInvokedElementName(invocation);

                        return invokedName.StartsWith("Method:Moq");
                    }
                }
            }

            return false;
        }
    }
}
