using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class DeclarationStatementEater : StatementEater<IDeclarationStatement>
    {
        public DeclarationStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IDeclarationStatement statement)
        {
            foreach (ILocalConstantDeclaration localConstantDeclaration in statement.ConstantDeclarations)
            {
                Eater.Eat(snapshot, localConstantDeclaration);
            }

            foreach (ILocalVariableDeclaration localVariableDeclaration in statement.VariableDeclarations)
            {
                Eater.Eat(snapshot, localVariableDeclaration);
            }
        }
    }
}