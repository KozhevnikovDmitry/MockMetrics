using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryGroupClause : QueryClauseEater<IQueryGroupClause>
    {
        public QueryGroupClause(IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IQueryGroupClause queryClause)
        {
            return Variable.None;
        }
    }
}