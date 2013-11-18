using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public class StubClauseEater : IQueryClauseEater
    {
        public ExpressionKind Eat(ISnapshot snapshot, IQueryClause queryClause)
        {
            return ExpressionKind.None;
        }

        public Type QueryClauseType { get; private set; }
    }
}