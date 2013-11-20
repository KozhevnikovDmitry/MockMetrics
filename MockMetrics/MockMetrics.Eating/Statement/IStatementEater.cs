using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public interface IStatementEater : ICSharpNodeEater
    {
        void Eat(ISnapshot snapshot, ICSharpStatement statement);

        Type StatementType { get; }
    }

    public interface IStatementEater<T> : IStatementEater where T : ICSharpStatement
    {
        void Eat(ISnapshot snapshot, T statement);
    }
}