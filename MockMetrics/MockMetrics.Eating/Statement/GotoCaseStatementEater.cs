using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class GotoCaseStatementEater : StatementEater<IGotoCaseStatement>
    {
        public GotoCaseStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IGotoCaseStatement statement)
        {
            ExpressionKind kind = Eater.Eat(snapshot, statement.ValueExpression);
            snapshot.AddTreeNode(kind, statement.ValueExpression);
        }
    }
}