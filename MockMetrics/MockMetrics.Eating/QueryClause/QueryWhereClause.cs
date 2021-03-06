﻿using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.QueryClause
{
    public class QueryWhereClause : QueryClauseEater<IQueryWhereClause>
    {
        public QueryWhereClause(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IQueryWhereClause queryClause)
        {
            return ExpressionKind.None;
        }
    }
}