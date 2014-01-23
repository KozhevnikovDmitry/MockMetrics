using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public interface IInitializerElementEater : INodeEater, ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IInitializerElement initializer);
    }

    public interface IInitializerElementEater<T> : IInitializerElementEater where T : IInitializerElement
    {
        Variable Eat(ISnapshot snapshot, T initializer);
    }
}