using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class DefaultExpressionEater : ExpressionEater<IDefaultExpression>
    {
        private readonly ITypeUsageEater _typeUsageEater;

        public DefaultExpressionEater(IEater eater, ITypeUsageEater typeUsageEater)
            : base(eater)
        {
            _typeUsageEater = typeUsageEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IDefaultExpression expression)
        {
            return _typeUsageEater.Eat(snapshot, expression.TypeName);
        }
    }
}
