using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public abstract class StatementEater<T> : NodeEater<T>, IStatementEater where T : ICSharpStatement
    {
        protected readonly IEater Eater;

        protected StatementEater([NotNull] IEater eater)
        {
            if (eater == null) 
                throw new ArgumentNullException("eater");
            Eater = eater;
        }

        public void Eat([NotNull] ISnapshot snapshot, [NotNull] ICSharpStatement statement)
        {
            EatNode(snapshot, statement, (s, n) => Eat(s, n));
        }

        public abstract void Eat(ISnapshot snapshot, T statement);

        public Type StatementType
        {
            get { return typeof (T); }
        }
    }
}