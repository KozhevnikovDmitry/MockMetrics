using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public interface IQueryClauseEater : INodeEater, ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IQueryClause queryClause);
    }

    public interface IQueryClauseEater<T> :  IQueryClauseEater where T : IQueryClause
    {
        Variable Eat(ISnapshot snapshot, T queryClause);
    }

    public abstract class QueryClauseEater<T> : NodeEater<T>, IQueryClauseEater<T> where T : IQueryClause
    {
        protected QueryClauseEater([NotNull] IEater eater)
            :base(eater)
        {

        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IQueryClause queryClause)
        {
            return EatNode(snapshot, queryClause, (s, n) => Eat(s, n));
        }

        public abstract Variable Eat(ISnapshot snapshot, T queryClause);
    }
}
