using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class StubExpressionEater : IExpressionEater
    {
        public StubExpressionEater()
        {
            ExpressionType = GetType();
        }

        public Metrics Eat(ISnapshot snapshot, ICSharpExpression statement)
        {
            return new Metrics();
        }

        public Type ExpressionType { get; private set; }
    }
}