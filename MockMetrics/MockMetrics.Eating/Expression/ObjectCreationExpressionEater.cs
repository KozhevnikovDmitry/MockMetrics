using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating.Expression
{
    public class ObjectCreationExpressionEater : ExpressionEater<IObjectCreationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;

        public ObjectCreationExpressionEater(IEater eater, EatExpressionHelper expressionHelper)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IObjectCreationExpression expression)
        {
            foreach (ICSharpArgument arg in expression.Arguments)
            {
                ExpressionKind kind = Eater.Eat(snapshot, arg.Value);
                if (kind != ExpressionKind.StubCandidate)
                {
                    snapshot.AddTreeNode(kind, arg);
                }
            }

            if (expression.Initializer != null)
            {
                foreach (var initializerElement in expression.Initializer.InitializerElements)
                {
                    // TODO : what is initializerElement?

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

                if (_expressionHelper.GetCreationTypeName(expression)
                                     .StartsWith("Moq.Mock"))
                {
                    return ExpressionKind.Mock;
                }
            }

            return ExpressionKind.StubCandidate;
        }

        private string GetProjectName(IObjectCreationExpression creationExpression)
        {
            if (creationExpression.Type().Classify == TypeClassification.REFERENCE_TYPE)
            {
                var classType = _expressionHelper.GetCreationClass(creationExpression);

                if (classType != null)
                {

                    return classType.Module.Name;
                }
            }

            return string.Empty;
        }
    }
}