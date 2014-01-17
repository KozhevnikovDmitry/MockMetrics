using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class UnsafeCodeFixedPointerInitializerEater : InitializerElementEater<IUnsafeCodeFixedPointerInitializer>
    {
        public UnsafeCodeFixedPointerInitializerEater([NotNull] IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IUnsafeCodeFixedPointerInitializer initializer)
        {
            return Eater.Eat(snapshot, initializer.Value);
        }
    }
}
