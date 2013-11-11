using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public interface IStatementEater
    {
        void Eat(Snapshot snapshot, ICSharpStatement statement);

        Type StatementType { get; }
    }

    public interface IStatementEater<T> : IStatementEater where T : ICSharpStatement
    {
        void Eat(Snapshot snapshot, T statement);
    }

    public abstract class StatementEater<T> : IStatementEater where T : ICSharpStatement
    {
        protected readonly Eater Eater;

        protected StatementEater(Eater eater)
        {
            Eater = eater;
        }

        public void Eat(Snapshot snapshot, ICSharpStatement statement)
        {
            if (statement is T)
            {
                Eat(snapshot, (T)statement);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public abstract void Eat(Snapshot snapshot, T statement);

        public Type StatementType
        {
            get { return typeof (T); }
        }
    }

   
}