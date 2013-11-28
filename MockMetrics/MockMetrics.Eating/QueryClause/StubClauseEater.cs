using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class StubClauseEater : IQueryClauseEater
    {
        public VarType Eat(ISnapshot snapshot, IQueryClause queryClause)
        {
            return VarType.None;
        }

        public Type QueryClauseType { get; private set; }
    }
}