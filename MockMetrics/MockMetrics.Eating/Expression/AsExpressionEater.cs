using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class AsExpressionEater : ExpressionEater<IAsExpression>
    {
        private readonly ITypeEater _typeEater;
        private readonly ExpressionKindHelper _kindHelper;

        public AsExpressionEater(IEater eater, ITypeEater typeEater, ExpressionKindHelper kindHelper)
            : base(eater)
        {
            _typeEater = typeEater;
            _kindHelper = kindHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IAsExpression expression)
        {
            var operandKind = Eater.Eat(snapshot, expression.Operand);
            var typeUsageKind = _typeEater.EatCastType(snapshot, expression.TypeOperand);
            return _kindHelper.ValueOfKindAsTypeOfKind(operandKind, typeUsageKind);
        }
    }
}
