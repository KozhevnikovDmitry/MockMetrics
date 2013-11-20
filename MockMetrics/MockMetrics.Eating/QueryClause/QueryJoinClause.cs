using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryJoinClause : QueryClauseEater<IQueryJoinClause>
    {
        public QueryJoinClause(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IQueryJoinClause queryClause)
        {
            return ExpressionKind.None;
        }
    }
}