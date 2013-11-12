using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalConstantDeclarationEater : VariableDeclarationEater<ILocalConstantDeclaration>
    {
        public LocalConstantDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILocalConstantDeclaration variableDeclaration)
        {
            snapshot.AddTreeNode(ExpressionKind.Stub, variableDeclaration);
        }
    }
}