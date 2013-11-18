using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class DefaultExpressionEater : ExpressionEater<IDefaultExpression>
    {
        private readonly ITypeEater _typeEater;

        public DefaultExpressionEater(IEater eater, ITypeEater typeEater)
            : base(eater)
        {
            _typeEater = typeEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IDefaultExpression expression)
        {
            return _typeEater.EatCastType(snapshot, expression.TypeName);
        }
    }
}
