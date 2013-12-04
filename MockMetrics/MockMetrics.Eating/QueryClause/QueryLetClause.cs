using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryLetClause : QueryClauseEater<IQueryLetClause>
    {
        public QueryLetClause(IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IQueryLetClause queryClause)
        {
            return Variable.None;
        }
    }
}