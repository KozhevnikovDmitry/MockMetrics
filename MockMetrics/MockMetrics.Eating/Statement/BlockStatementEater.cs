using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    /// <summary>
    ///     Just eat all statements
    /// </summary>
    public class BlockStatementEater : IStatementEater<IBlock>
    {
        public void Eat(Snapshot snapshot, IMethodDeclaration unitTest, IBlock statement)
        {
            foreach (ICSharpStatement inBlockStatement in statement.Statements.OfType<ICSharpStatement>())
            {
                Eater.Eat(snapshot, unitTest, inBlockStatement);
            }
        }
    }
}