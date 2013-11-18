using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public interface IQueryClauseEater
    {
        ExpressionKind Eat(ISnapshot snapshot, IQueryClause queryClause);

        Type QueryClauseType { get; }
    }

    public interface IQueryClauseEater<T> : IQueryClauseEater where T : IQueryClause
    {
        ExpressionKind Eat(ISnapshot snapshot, T queryClause);
    }

    public abstract class QueryClauseEater<T> : IQueryClauseEater<T> where T : IQueryClause
    {
        protected readonly IEater Eater;

        protected QueryClauseEater(IEater eater)
        {
            Eater = eater;
        }

        public ExpressionKind Eat(ISnapshot snapshot, IQueryClause queryClause)
        {
            if (queryClause is T)
            {
                return Eat(snapshot, (T)queryClause);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public abstract ExpressionKind Eat(ISnapshot snapshot, T queryClause);

        public Type QueryClauseType
        {
            get { return typeof(T); }
        }
    }
}
