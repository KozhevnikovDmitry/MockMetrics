using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ObjectCreationExpressionEater : ExpressionEater<IObjectCreationExpression>
    {
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMetricHelper _metricHelper;

        public ObjectCreationExpressionEater(IEater eater, 
                                             IArgumentsEater argumentsEater,
                                             IMetricHelper metricHelper)
            : base(eater)
        {
            _argumentsEater = argumentsEater;
            _metricHelper = metricHelper;
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

            return _metricHelper.MetricsForType(snapshot, expression.Type());
        }
    }
}