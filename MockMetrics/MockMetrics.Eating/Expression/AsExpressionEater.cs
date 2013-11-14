using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class AsExpressionEater : ExpressionEater<IAsExpression>
    {
        private readonly ITypeUsageEater _typeUsageEater;
        private readonly ExpressionKindHelper _kindHelper;

        public AsExpressionEater(IEater eater, ITypeUsageEater typeUsageEater, ExpressionKindHelper kindHelper)
            : base(eater)
        {
            _typeUsageEater = typeUsageEater;
            _kindHelper = kindHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IAsExpression expression)
        {
            var operandKind = Eater.Eat(snapshot, expression.Operand);
            var typeUsageKind = _typeUsageEater.Eat(snapshot, expression.TypeOperand);
            return _kindHelper.ValueOfKindAsTypeOfKind(operandKind, typeUsageKind);
        }
    }
}
