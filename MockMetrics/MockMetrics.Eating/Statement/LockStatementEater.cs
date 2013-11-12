using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class LockStatementEater : StatementEater<ILockStatement>
    {
        public LockStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILockStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            var condKind = Eater.Eat(snapshot, statement.Monitor);
            snapshot.AddTreeNode(condKind, statement.Monitor);
        }
    }
}