using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ObjectCreationExpressionEater : ExpressionEater<IObjectCreationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly IArgumentsEater _argumentsEater;
        private readonly ITypeEater _typeEater;

        public ObjectCreationExpressionEater(IEater eater, 
                                             EatExpressionHelper expressionHelper, 
                                             IArgumentsEater argumentsEater,
                                             ITypeEater typeEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _argumentsEater = argumentsEater;
            _typeEater = typeEater;
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

            return _typeEater.VarTypeVariableType(snapshot, expression.Type());
        }
    }
}