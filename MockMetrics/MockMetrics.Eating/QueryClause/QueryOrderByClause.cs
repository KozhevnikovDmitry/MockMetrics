using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryOrderByClause : QueryClauseEater<IQueryOrderByClause>
    {
        public QueryOrderByClause(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IQueryOrderByClause queryClause)
        {
            return VarType.None;
        }
    }
}