using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    /// <summary>
    ///     All literals are stubs
    /// </summary>
    public class LiteralExpressionEater : ExpressionEater<ICSharpLiteralExpression>
    {
        public LiteralExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, ICSharpLiteralExpression expression)
        {
            return ExpressionKind.Stub;
        }
    }
}