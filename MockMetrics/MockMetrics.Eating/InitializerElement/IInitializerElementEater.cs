using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public interface IInitializerElementEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IInitializerElement initializer);

        Type InitializerElementType { get; }
    }

    public interface IInitializerElementEater<T> : IInitializerElementEater where T : IInitializerElement
    {
        Variable Eat(ISnapshot snapshot, T initializer);
    }
}