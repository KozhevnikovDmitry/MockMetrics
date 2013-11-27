using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AsExpressionEater : ExpressionEater<IAsExpression>
    {
        private readonly ITypeEater _typeEater;
        private readonly MetricHelper _metricHelper;

        public AsExpressionEater(IEater eater, ITypeEater typeEater, MetricHelper metricHelper)
            : base(eater)
        {
            _typeEater = typeEater;
            _metricHelper = metricHelper;
        }

        public override VarType Eat(ISnapshot snapshot, IAsExpression expression)
        {
            snapshot.AddOperand(expression.TypeOperand, Scope.Local, Aim.None, VarType.Library);

            var operandKind = Eater.Eat(snapshot, expression.Operand);
            var typeUsageKind = _typeEater.EatCastType(snapshot, expression.TypeOperand);
            return _metricHelper.CastExpressionType(operandKind, typeUsageKind);
        }
    }
}
