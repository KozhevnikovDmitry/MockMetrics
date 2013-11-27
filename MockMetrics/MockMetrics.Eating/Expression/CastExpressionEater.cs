using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class CastExpressionEater : ExpressionEater<ICastExpression>
    {
        private readonly ITypeEater _typeEater;
        private readonly VarTypeHelper _kindHelper;

        public CastExpressionEater(IEater eater, ITypeEater typeEater, VarTypeHelper kindHelper)
            : base(eater)
        {
            _typeEater = typeEater;
            _kindHelper = kindHelper;
        }

        public override VarType Eat(ISnapshot snapshot, ICastExpression expression)
        {
            var operandKind = Eater.Eat(snapshot, expression.Op);
            var typeUsageKind = _typeEater.EatCastType(snapshot, expression.TargetType);
            return _kindHelper.CastExpressionType(operandKind, typeUsageKind);
        }
    }
}