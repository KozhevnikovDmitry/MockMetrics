using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public interface IExpressionEater : INodeEater, ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, ICSharpExpression statement);
    }

    public interface IExpressionEater<T> : IExpressionEater where T : ICSharpExpression
    {
        Variable Eat(ISnapshot snapshot, T expression);
    }
}
