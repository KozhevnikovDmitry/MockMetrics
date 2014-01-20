using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class AnonymousMemberDeclarationEater : InitializerElementEater<IAnonymousMemberDeclaration>
    {
        public AnonymousMemberDeclarationEater([NotNull] IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IAnonymousMemberDeclaration initializer)
        {
            var memberMetrics = Eater.Eat(snapshot, initializer.Expression);
            snapshot.AddVariable(initializer, memberMetrics);
            return memberMetrics;
        }
    }
}
