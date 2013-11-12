using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class DoStatementEater : StatementEater<IDoStatement>
    {
        public DoStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IDoStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            var condKind = Eater.Eat(snapshot, statement.Condition);
            snapshot.AddTreeNode(condKind, statement.Condition);
        }
    }
}