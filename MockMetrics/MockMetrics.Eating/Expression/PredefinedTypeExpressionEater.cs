using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class PredefinedTypeExpressionEater : ExpressionEater<IPredefinedTypeExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public PredefinedTypeExpressionEater([NotNull] IEater eater, IMetricHelper metricHelper) 
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IPredefinedTypeExpression expression)
        {
            return Variable.Library;
            //return _metricHelper.MetricForTypeReferece(snapshot, expression.PredefinedTypeName.)
        }
    }
}
