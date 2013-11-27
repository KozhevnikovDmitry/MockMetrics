using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ObjectCreationExpressionEater : ExpressionEater<IObjectCreationExpression>
    {
        private readonly IArgumentsEater _argumentsEater;
        private readonly ITypeHelper _typeHelper;

        public ObjectCreationExpressionEater(IEater eater, 
                                             IArgumentsEater argumentsEater,
                                             ITypeHelper typeHelper)
            : base(eater)
        {
            _argumentsEater = argumentsEater;
            _typeHelper = typeHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IObjectCreationExpression expression)
        {
            _argumentsEater.Eat(snapshot, expression.Arguments);
            
            if (expression.Initializer != null)
            {
                foreach (IMemberInitializer memberInitializer in expression.Initializer.InitializerElements)
                {
                   Eater.Eat(snapshot, memberInitializer.Expression);
                }
            }

            return _typeHelper.MetricVariable(snapshot, expression.Type());
        }
    }
}