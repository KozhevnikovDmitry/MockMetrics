using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class MemberInitializerEater : InitializerElementEater<IMemberInitializer>
    {
        public MemberInitializerEater([NotNull] IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IMemberInitializer initializer)
        {
            var varType = Eater.Eat(snapshot, initializer.Expression);

            snapshot.AddVariable(initializer, varType);

            return varType;
        }
    }
}
