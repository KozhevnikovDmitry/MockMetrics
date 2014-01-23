using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public abstract class VariableDeclarationEater<T> : NodeEater<T>, IVariableDeclarationEater<T> where T : IVariableDeclaration
    {
        protected VariableDeclarationEater([NotNull] IEater eater)
            : base(eater)
        {

        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IVariableDeclaration variableDeclaration)
        {
            return EatNode(snapshot, variableDeclaration, (s, n) => Eat(s, n));
        }

        public abstract Variable Eat(ISnapshot snapshot, T variableDeclaration);
    }
}