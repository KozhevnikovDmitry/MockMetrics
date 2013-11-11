using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    /// <summary>
    /// Just eat all statements
    /// </summary>
    public class BlockStatementEater : StatementEater<IBlock>
    {
        public BlockStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IBlock statement)
        {
            foreach (ICSharpStatement inBlockStatement in statement.Statements.OfType<ICSharpStatement>())
            {
                Eater.Eat(snapshot, inBlockStatement);
            }
        }
    }
}