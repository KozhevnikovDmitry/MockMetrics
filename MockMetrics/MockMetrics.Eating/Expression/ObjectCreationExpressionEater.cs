using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

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

        public override VarType Eat(ISnapshot snapshot, IObjectCreationExpression expression)
        {
            _argumentsEater.Eat(snapshot, expression.Arguments);
            
            if (expression.Initializer != null)
            {
                foreach (IMemberInitializer memberInitializer in expression.Initializer.InitializerElements)
                {
                   Eater.Eat(snapshot, memberInitializer.Expression);
                }
            }

            return GetCreationObjectKind(snapshot, expression);
        }

        private VarType GetCreationObjectKind(ISnapshot snapshot, IObjectCreationExpression expression)
        {
            if (expression.Type().Classify == TypeClassification.REFERENCE_TYPE)
            {
                var projectName = GetProjectName(expression);

                if (snapshot.IsInTestScope(projectName))
                {
                    return VarType.Target;
                }

                if (snapshot.IsInTestProject(projectName))
                {
                    return VarType.Mock;
                }

                if (_expressionHelper.GetCreationTypeName(expression)
                                     .StartsWith("Moq.Mock"))
                {
                    return VarType.Mock;
                }
            }

            return VarType.Library;
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