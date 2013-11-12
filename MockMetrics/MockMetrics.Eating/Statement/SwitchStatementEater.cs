using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class SwitchStatementEater : StatementEater<ISwitchStatement>
    {
        public SwitchStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ISwitchStatement statement)
        {
            Eater.Eat(snapshot, statement.Block);

            var condKind = Eater.Eat(snapshot, statement.Condition);
            snapshot.AddTreeNode(condKind, statement.Condition);
        }
    }
}
