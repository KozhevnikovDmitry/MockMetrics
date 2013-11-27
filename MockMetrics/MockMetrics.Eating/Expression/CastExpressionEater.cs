using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class CastExpressionEater : ExpressionEater<ICastExpression>
    {
        private readonly ITypeEater _typeEater;
        private readonly MetricHelper _metricHelper;

        public CastExpressionEater(IEater eater, ITypeEater typeEater, MetricHelper metricHelper)
            : base(eater)
        {
            _typeEater = typeEater;
            _metricHelper = metricHelper;
        }

        public override VarType Eat(ISnapshot snapshot, ICastExpression expression)
        {
            snapshot.AddOperand(expression.TargetType, Scope.Local, Aim.None, VarType.Library);

            var operandKind = Eater.Eat(snapshot, expression.Op);
            var typeUsageKind = _typeEater.EatCastType(snapshot, expression.TargetType);
            return _metricHelper.CastExpressionType(operandKind, typeUsageKind);
        }
    }
}