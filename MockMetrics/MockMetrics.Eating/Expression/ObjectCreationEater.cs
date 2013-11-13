using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl.reflection2.elements.Context;

namespace MockMetrics.Eating.Expression
{
    public class ObjectCreationEater : ExpressionEater<IObjectCreationExpression>
    {
        public ObjectCreationEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IObjectCreationExpression expression)
        {
            foreach (ICSharpArgument arg in expression.Arguments)
            {
                ExpressionKind kind = Eater.Eat(snapshot, arg.Value);
                snapshot.AddTreeNode(kind, arg);
            }

            if (IsStubCreation(expression))
            {
                return ExpressionKind.Stub;
            }

            if (IsMockCreation(expression))
            {
                return ExpressionKind.Mock;
            }

            if (IsTargetCreation(expression, snapshot))
            {
                return ExpressionKind.Target;
            }

            throw new NotSupportedException();
        }

        private bool IsStubCreation(IObjectCreationExpression creationExpression)
        {
            return creationExpression.Type().Classify == TypeClassification.VALUE_TYPE;
        }

        private bool IsMockCreation(IObjectCreationExpression creationExpression)
        {
            if (creationExpression.Type().Classify == TypeClassification.REFERENCE_TYPE)
            {
                if (creationExpression.TypeReference
                    .CurrentResolveResult
                    .DeclaredElement
                    .ToString()
                    .StartsWith("Moq.Mock"))
                {
                    return true;
                }

                string projectName;

                var classType = creationExpression.TypeReference.CurrentResolveResult.DeclaredElement as Class;

                if (classType == null)
                {
                    var classElement =
                        creationExpression.TypeReference.CurrentResolveResult.DeclaredElement as ClassElement;

                    if (classElement == null)
                    {
                        return false;
                    }

                    projectName = classElement.Module.DisplayName;
                }
                else
                {
                    projectName = classType.Module.DisplayName;
                }


                return projectName == creationExpression.Parent.GetSourceFile().PsiModule.DisplayName;
            }

            return false;
        }

        private bool IsTargetCreation(IObjectCreationExpression creationExpression, ISnapshot snapshot)
        {
            if (creationExpression.Type().Classify == TypeClassification.REFERENCE_TYPE)
            {
                string projectName;

                var classType = creationExpression.TypeReference.CurrentResolveResult.DeclaredElement as Class;

                if (classType == null)
                {
                    var classElement =
                        creationExpression.TypeReference.CurrentResolveResult.DeclaredElement as ClassElement;

                    if (classElement == null)
                    {
                        return false;
                    }

                    projectName = classElement.Module.Name;
                }
                else
                {
                    projectName = classType.Module.Name;
                }


                return snapshot.IsInTestScope(projectName);
            }

            return false;
        }
    }
}