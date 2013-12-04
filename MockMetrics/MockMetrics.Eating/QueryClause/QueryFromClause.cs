using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryFromClause : QueryClauseEater<IQueryFromClause>
    {
        public QueryFromClause(IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IQueryFromClause queryClause)
        {
            return Variable.None;
        }
    }
}