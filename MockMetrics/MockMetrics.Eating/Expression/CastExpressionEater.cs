using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class CastExpressionEater : ExpressionEater<ICastExpression>
    {
        private readonly ITypeUsageEater _typeUsageEater;
        private readonly ExpressionKindHelper _kindHelper;

        public CastExpressionEater(IEater eater, ITypeUsageEater typeUsageEater, ExpressionKindHelper kindHelper)
            : base(eater)
        {
            _typeUsageEater = typeUsageEater;
            _kindHelper = kindHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, ICastExpression expression)
        {
            var operandKind = Eater.Eat(snapshot, expression.Op);
            var typeUsageKind = _typeUsageEater.Eat(snapshot, expression.TargetType);
            return _kindHelper.ValueOfKindAsTypeOfKind(operandKind, typeUsageKind);
        }
    }
}