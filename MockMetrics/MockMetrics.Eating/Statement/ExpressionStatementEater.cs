using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class ExpressionStatementEater : StatementEater<IExpressionStatement>
    {
        public ExpressionStatementEater(Eater eater)
            : base(eater)
        {
        }

        public override void Eat(Snapshot snapshot, IExpressionStatement statement)
        {
            ExpressionKind kind = Eater.Eat(snapshot, statement.Expression);
            snapshot.AddTreeNode(kind, statement);
        }
    }
}