using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class ExpressionStatementEater : StatementEater<IExpressionStatement>
    {
        public ExpressionStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IExpressionStatement statement)
        {
            ExpressionKind kind = Eater.Eat(snapshot, statement.Expression);
            snapshot.AddTreeNode(kind, statement);
        }
    }
}