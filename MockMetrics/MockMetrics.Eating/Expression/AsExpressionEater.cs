using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AsExpressionEater : ExpressionEater<IAsExpression>
    {
        private readonly ITypeHelper _typeHelper;
        private readonly MetricHelper _metricHelper;

        public AsExpressionEater(IEater eater, ITypeHelper typeHelper, MetricHelper metricHelper)
            : base(eater)
        {
            _typeHelper = typeHelper;
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IAsExpression expression)
        {
            snapshot.AddOperand(expression.TypeOperand, Metrics.Create(Scope.Local, VarType.Library));

            var operandMetrics = Eater.Eat(snapshot, expression.Operand);
            var typeVarType = _typeHelper.MetricCastType(snapshot, expression.TypeOperand);

            return Metrics.Create(_metricHelper.CastExpressionType(operandMetrics, typeVarType));
        }
    }
}
