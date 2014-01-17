using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ObjectCreationExpressionEater : ExpressionEater<IObjectCreationExpression>
    {
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public ObjectCreationExpressionEater(IEater eater, 
                                             IArgumentsEater argumentsEater,
                                             IMetricHelper metricHelper,
                                             EatExpressionHelper eatExpressionHelper)
            : base(eater)
        {
            _argumentsEater = argumentsEater;
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IObjectCreationExpression expression)
        {
            _argumentsEater.Eat(snapshot, expression.Arguments);
            
            if (expression.Initializer != null)
            {
                // TODO : Cover by unit tests
                foreach (IInitializerElement initializer in expression.Initializer.InitializerElements)
                {
                    Eater.Eat(snapshot, initializer);
                }
            }

            var vartype = _metricHelper.MetricsForType(snapshot, expression.Type());

            if (_eatExpressionHelper.IsStandaloneObjectCreationExpression(expression))
            {
                snapshot.AddVariable(expression, vartype);
            }

            return vartype;
        }
    }
}