using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryLetClause : QueryClauseEater<IQueryLetClause>
    {
        public QueryLetClause(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IQueryLetClause queryClause)
        {
            return ExpressionKind.None;
        }
    }
}