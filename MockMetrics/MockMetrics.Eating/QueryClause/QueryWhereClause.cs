using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryWhereClause : QueryClauseEater<IQueryWhereClause>
    {
        public QueryWhereClause(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IQueryWhereClause queryClause)
        {
            return VarType.None;
        }
    }
}