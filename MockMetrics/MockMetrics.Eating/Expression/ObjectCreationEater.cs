using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

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

            if (expression.Initializer != null)
            {
                foreach (var initializerElement in expression.Initializer.InitializerElements)
                {

                }
            }

            return GetCreationObjectKind(snapshot, expression);
        }

        private ExpressionKind GetCreationObjectKind(ISnapshot snapshot, IObjectCreationExpression expression)
        {
            if (expression.Type().Classify == TypeClassification.REFERENCE_TYPE)
            {
                var projectName = GetProjectName(expression);

                if (snapshot.IsInTestScope(projectName))
                {
                    return ExpressionKind.Target;
                }

                if (snapshot.IsInTestProject(projectName))
                {
                    return ExpressionKind.Mock;
                }

                if (expression.Type()
                              .ToString()
                              .StartsWith("Moq.Mock"))
                {
                    return ExpressionKind.Mock;
                }
            }

            return ExpressionKind.Stub;
        }

        private string GetProjectName(IObjectCreationExpression creationExpression)
        {
            if (creationExpression.Type().Classify == TypeClassification.REFERENCE_TYPE)
            {
                var classType = creationExpression.TypeReference.CurrentResolveResult.DeclaredElement as IClass;

                if (classType != null)
                {

                    return classType.Module.Name;
                }
            }

            return string.Empty;
        }
    }
}