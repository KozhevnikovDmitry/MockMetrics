using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics
{
    public interface IExpressionEater<T> where T : ICSharpExpression
    {
        Snapshot Eat(Snapshot snapshot, IMethodDeclaration test, T statement);
    }
}