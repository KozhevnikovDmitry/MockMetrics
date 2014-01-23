using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class StubStatementEater : IStatementEater
    {
        public StubStatementEater()
        {
            NodeType = GetType();
        }

        public void Eat(ISnapshot snapshot, ICSharpStatement statement)
        {
            
        }

        public Type NodeType { get; private set; }
    }
}