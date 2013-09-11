using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eat
{
    public interface IEater<T> where T : ICSharpTreeNode
    {
        Snapshot Eat(Snapshot snapshot, IMethodDeclaration unitTest, T parameterDeclaration);
    }
}