using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class ReturnStatementEater : StatementEater<IReturnStatement>
    {
        public ReturnStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IReturnStatement statement)
        {
            var condKind = Eater.Eat(snapshot, statement.Value);
            snapshot.Add(condKind, statement.Value);
        }
    }
}