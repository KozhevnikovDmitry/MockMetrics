using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class ExpressionStatementEater : IStatementEater<IExpressionStatement>
    {
        public void Eat(Snapshot snapshot, IMethodDeclaration unitTest, IExpressionStatement statement)
        {
            ExpressionKind kind = Eater.Eat(snapshot, unitTest, statement.Expression);
            snapshot.AddTreeNode(kind, statement);
        }
    }
}