using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryGroupClause : QueryClauseEater<IQueryGroupClause>
    {
        public QueryGroupClause(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IQueryGroupClause queryClause)
        {
            return VarType.None;
        }
    }
}