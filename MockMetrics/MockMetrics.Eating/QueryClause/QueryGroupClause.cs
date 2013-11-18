using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryGroupClause : QueryClauseEater<IQueryGroupClause>
    {
        public QueryGroupClause(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IQueryGroupClause queryClause)
        {
            return ExpressionKind.None;
        }
    }
}