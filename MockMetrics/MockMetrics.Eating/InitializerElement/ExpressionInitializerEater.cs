using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class ExpressionInitializerEater : InitializerElementEater<IExpressionInitializer>
    {
        public ExpressionInitializerEater([NotNull] IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IExpressionInitializer initializer)
        {
            return Eater.Eat(snapshot, initializer.Value);
        }
    }
}
