using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class YieldStatementEater : StatementEater<IYieldStatement>
    {
        public YieldStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IYieldStatement statement)
        {
            var condKind = Eater.Eat(snapshot, statement.Expression);
            snapshot.AddTreeNode(condKind, statement.Expression);
        }
    }
}