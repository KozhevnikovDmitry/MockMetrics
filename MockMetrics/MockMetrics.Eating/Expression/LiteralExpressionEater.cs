using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    /// <summary>
    ///     All literals are stubs
    /// </summary>
    public class LiteralExpressionEater : IExpressionEater<ICSharpLiteralExpression>
    {
        public ExpressionKind Eat(Snapshot snapshot, IMethodDeclaration unitTest, ICSharpLiteralExpression expression)
        {
            return ExpressionKind.Stub;
        }
    }
}