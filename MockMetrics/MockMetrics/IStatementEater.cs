using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics
{
    public interface IStatementEater<T> where T : ICSharpStatement
    {
        Snapshot Eat(Snapshot snapshot, IMethodDeclaration test, T statement);
    }
}
