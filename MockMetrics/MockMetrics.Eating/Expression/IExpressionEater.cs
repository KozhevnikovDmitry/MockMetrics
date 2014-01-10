using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public interface IExpressionEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, ICSharpExpression statement);

        Type ExpressionType { get; }
    }

    public interface IExpressionEater<T> : IExpressionEater where T : ICSharpExpression
    {
        Variable Eat(ISnapshot snapshot, T expression);
    }
}
