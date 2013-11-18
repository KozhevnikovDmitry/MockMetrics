using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class CastExpressionEater : ExpressionEater<ICastExpression>
    {
        private readonly ITypeEater _typeEater;
        private readonly ExpressionKindHelper _kindHelper;

        public CastExpressionEater(IEater eater, ITypeEater typeEater, ExpressionKindHelper kindHelper)
            : base(eater)
        {
            _typeEater = typeEater;
            _kindHelper = kindHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, ICastExpression expression)
        {
            var operandKind = Eater.Eat(snapshot, expression.Op);
            var typeUsageKind = _typeEater.EatCastType(snapshot, expression.TargetType);
            return _kindHelper.ValueOfKindAsTypeOfKind(operandKind, typeUsageKind);
        }
    }
}