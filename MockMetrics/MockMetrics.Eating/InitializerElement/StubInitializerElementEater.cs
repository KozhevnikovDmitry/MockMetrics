using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class StubInitializerElementEater : IInitializerElementEater 
    {
        public Variable Eat(ISnapshot snapshot, IInitializerElement initializer)
        {
            return Variable.None;
        }

        public Type InitializerElementType { get; private set; }
    }
}