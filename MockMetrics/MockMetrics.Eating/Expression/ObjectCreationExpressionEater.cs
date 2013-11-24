using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ObjectCreationExpressionEater : ExpressionEater<IObjectCreationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly IArgumentsEater _argumentsEater;

        public ObjectCreationExpressionEater(IEater eater, EatExpressionHelper expressionHelper, IArgumentsEater argumentsEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _argumentsEater = argumentsEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IObjectCreationExpression expression, bool innerEat)
        {
            _argumentsEater.Eat(snapshot, expression.Arguments);
            
            if (expression.Initializer != null)
            {
                foreach (IMemberInitializer memberInitializer in expression.Initializer.InitializerElements)
                {
                   Eater.Eat(snapshot, memberInitializer.Expression, true);
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