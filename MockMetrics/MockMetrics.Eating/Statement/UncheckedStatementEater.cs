using JetBrains.ReSharper.Psi.CSharp.Tree;

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
}
