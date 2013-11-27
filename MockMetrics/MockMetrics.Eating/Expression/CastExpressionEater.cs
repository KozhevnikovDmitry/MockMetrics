using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class CastExpressionEater : ExpressionEater<ICastExpression>
    {
        private readonly ITypeHelper _typeHelper;
        private readonly MetricHelper _metricHelper;

        public CastExpressionEater(IEater eater, ITypeHelper typeHelper, MetricHelper metricHelper)
            : base(eater)
        {
            _typeHelper = typeHelper;
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, ICastExpression expression)
        {
            snapshot.AddOperand(expression.TargetType, Metrics.Create(Scope.Local, VarType.Library));

            var operandMetrics = Eater.Eat(snapshot, expression.Op);
            var typeVarType = _typeHelper.MetricCastType(snapshot, expression.TargetType);

            return Metrics.Create(_metricHelper.CastExpressionType(operandMetrics, typeVarType));
        }
    }
}