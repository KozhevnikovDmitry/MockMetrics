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

        public VarType Eat(ISnapshot snapshot, ICSharpExpression statement)
        {
            return VarType.None;
        }

        public Type ExpressionType { get; private set; }
    }
}