using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AsExpressionEater : ExpressionEater<IAsExpression>
    {
        private readonly ITypeEater _typeEater;
        private readonly VarTypeHelper _kindHelper;

        public AsExpressionEater(IEater eater, ITypeEater typeEater, VarTypeHelper kindHelper)
            : base(eater)
        {
            _typeEater = typeEater;
            _kindHelper = kindHelper;
        }

        public override VarType Eat(ISnapshot snapshot, IAsExpression expression)
        {
            var operandKind = Eater.Eat(snapshot, expression.Operand);
            var typeUsageKind = _typeEater.EatCastType(snapshot, expression.TypeOperand);
            return _kindHelper.CastExpressionType(operandKind, typeUsageKind);
        }
    }
}
