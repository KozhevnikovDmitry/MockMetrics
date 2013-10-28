using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public interface IStatementEater<T> where T : ICSharpStatement
    {
        void Eat(Snapshot snapshot, IMethodDeclaration unitTest, T statement);
    }

    public interface IExpressionEater<T> where T : ICSharpExpression
    {
        ExpressionKind Eat(Snapshot snapshot, IMethodDeclaration unitTest, T expression);
    }
}