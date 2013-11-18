using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryFromClause : QueryClauseEater<IQueryFromClause>
    {
        public QueryFromClause(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IQueryFromClause queryClause)
        {
            return ExpressionKind.None;
        }
    }
}