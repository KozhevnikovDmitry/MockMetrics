using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class DefaultExpressionEater : ExpressionEater<IDefaultExpression>
    {
        private readonly ITypeHelper _typeHelper;

        public DefaultExpressionEater(IEater eater, ITypeHelper typeHelper)
            : base(eater)
        {
            _typeHelper = typeHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IDefaultExpression expression)
        {
            return Metrics.Create(_typeHelper.MetricCastType(snapshot, expression.TypeName));
        }
    }
}
