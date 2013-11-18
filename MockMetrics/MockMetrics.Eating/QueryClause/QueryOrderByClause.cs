using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryOrderByClause : QueryClauseEater<IQueryOrderByClause>
    {
        public QueryOrderByClause(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IQueryOrderByClause queryClause)
        {
            return ExpressionKind.None;
        }
    }
}