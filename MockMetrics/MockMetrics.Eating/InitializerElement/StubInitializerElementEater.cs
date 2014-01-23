using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class StubInitializerElementEater : IInitializerElementEater 
    {
        public StubInitializerElementEater()
        {
            NodeType = GetType();
        }

        public Variable Eat(ISnapshot snapshot, IInitializerElement initializer)
        {
            return Variable.None;
        }

        public Type NodeType { get; private set; }
    }
}