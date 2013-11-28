using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryJoinClause : QueryClauseEater<IQueryJoinClause>
    {
        public QueryJoinClause(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IQueryJoinClause queryClause)
        {
            return VarType.None;
        }
    }
}