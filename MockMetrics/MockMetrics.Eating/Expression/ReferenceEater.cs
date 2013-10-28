using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceEater : IExpressionEater<IReferenceExpression>
    {
        public ExpressionKind Eat(Snapshot snapshot, IMethodDeclaration unitTest, IReferenceExpression expression)
        {
            return ExpressionKind.None;
        }
    }
}