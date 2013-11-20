using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class CatchVariableDeclarationEater : VariableDeclarationEater<ICatchVariableDeclaration>
    {
        public CatchVariableDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ICatchVariableDeclaration variableDeclaration)
        {
            snapshot.Add(variableDeclaration);
        }
    }
}
