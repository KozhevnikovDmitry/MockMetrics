using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqFake
{
    public interface IMoqInvocationEater
    {
        Variable Eat(ISnapshot snapshot, IInvocationExpression expression);

        bool ContainsFakeOptions(IInvocationExpression expression);
    }
}