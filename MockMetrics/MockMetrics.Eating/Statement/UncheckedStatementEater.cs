using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class UncheckedStatementEater : StatementEater<IUncheckedStatement>
    {
        public UncheckedStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IUncheckedStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);
        }
    }

    public class UnsafeCodeUnsafeStatementEater : StatementEater<IUnsafeCodeUnsafeStatement>
    {
        public UnsafeCodeUnsafeStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IUnsafeCodeUnsafeStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);
        }
    }

    public class UnsafeCodeFixedStatementEater : StatementEater<IUnsafeCodeFixedStatement>
    {
        public UnsafeCodeFixedStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IUnsafeCodeFixedStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            foreach (var declaration in statement.PointerDeclarations)
            {
                Eater.Eat(snapshot, declaration);
            }
        }
    }
}
