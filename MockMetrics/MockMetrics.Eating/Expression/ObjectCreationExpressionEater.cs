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
                foreach (IMemberInitializer memberInitializer in expression.Initializer.InitializerElements)
                {
                    var varType = Eater.Eat(snapshot, memberInitializer.Expression);

                    snapshot.AddVariable(memberInitializer, varType);
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