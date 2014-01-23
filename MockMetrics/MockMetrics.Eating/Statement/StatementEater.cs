using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public abstract class StatementEater<T> : NodeEater<T>, IStatementEater where T : ICSharpStatement
    {
        protected StatementEater([NotNull] IEater eater)
            : base(eater)
        {

        }

        public void Eat([NotNull] ISnapshot snapshot, [NotNull] ICSharpStatement statement)
        {
            EatNode(snapshot, statement, (s, n) => Eat(s, n));
        }

        public abstract void Eat(ISnapshot snapshot, T statement);
    }
}