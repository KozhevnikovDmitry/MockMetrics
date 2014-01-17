using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class UnsafeCodeStackAllocInitializerEater : InitializerElementEater<IUnsafeCodeStackAllocInitializer>
    {
        public UnsafeCodeStackAllocInitializerEater([NotNull] IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IUnsafeCodeStackAllocInitializer initializer)
        {
            return Eater.Eat(snapshot, initializer.DimExpr);
        }
    }
}
