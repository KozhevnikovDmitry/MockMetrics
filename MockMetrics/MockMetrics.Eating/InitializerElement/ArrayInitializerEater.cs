using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class ArrayInitializerEater : InitializerElementEater<IArrayInitializer>
    {
        public ArrayInitializerEater([NotNull] IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IArrayInitializer initializer)
        {
            foreach (IVariableInitializer variableInitializer in initializer.ElementInitializers)
            {
                Eater.Eat(snapshot, variableInitializer);
            }

            return Variable.Library;
        }
    }
}
