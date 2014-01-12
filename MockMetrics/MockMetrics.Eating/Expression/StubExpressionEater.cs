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

        public Variable Eat(ISnapshot snapshot, ICSharpExpression statement)
        {
            return Variable.None;
        }

        public Type ExpressionType { get; private set; }
    }
}