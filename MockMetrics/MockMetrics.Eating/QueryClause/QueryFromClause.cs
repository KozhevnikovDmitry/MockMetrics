using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryFromClause : QueryClauseEater<IQueryFromClause>
    {
        public QueryFromClause(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IQueryFromClause queryClause)
        {
            return VarType.None;
        }
    }
}