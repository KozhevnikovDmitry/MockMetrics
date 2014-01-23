using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public class StubQueryClauseEater : IQueryClauseEater
    {
        public StubQueryClauseEater()
        {
            NodeType = GetType();
        }

        public Variable Eat(ISnapshot snapshot, IQueryClause queryClause)
        {
            return Variable.None;
        }

        public Type NodeType { get; private set; }
    }
}