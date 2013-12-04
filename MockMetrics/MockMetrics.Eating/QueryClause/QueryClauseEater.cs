using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public interface IQueryClauseEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IQueryClause queryClause);

        Type QueryClauseType { get; }
    }

    public interface IQueryClauseEater<T> : IQueryClauseEater where T : IQueryClause
    {
        Variable Eat(ISnapshot snapshot, T queryClause);
    }

    public abstract class QueryClauseEater<T> : IQueryClauseEater<T> where T : IQueryClause
    {
        protected readonly IEater Eater;

        protected QueryClauseEater([NotNull] IEater eater)
        {
            if (eater == null)
                throw new ArgumentNullException("eater");

            Eater = eater;
        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IQueryClause queryClause)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (queryClause == null) 
                throw new ArgumentNullException("queryClause");

            try
            {
                if (queryClause is T)
                {
                    return Eat(snapshot, (T)queryClause);
                }

                throw new UnexpectedTypeOfNodeToEatException(typeof(T), this, queryClause);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EatingException("Unexpected exception", ex, this, queryClause);
            }
        }

        public abstract Variable Eat(ISnapshot snapshot, T queryClause);

        public Type QueryClauseType
        {
            get { return typeof(T); }
        }
    }
}
