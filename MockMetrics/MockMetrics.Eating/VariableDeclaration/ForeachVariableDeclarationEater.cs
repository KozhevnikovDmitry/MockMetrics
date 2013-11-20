using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class ForeachVariableDeclarationEater : VariableDeclarationEater<IForeachVariableDeclaration>
    {
        public ForeachVariableDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IForeachVariableDeclaration variableDeclaration)
        {
            snapshot.Add(variableDeclaration);
        }
    }
}