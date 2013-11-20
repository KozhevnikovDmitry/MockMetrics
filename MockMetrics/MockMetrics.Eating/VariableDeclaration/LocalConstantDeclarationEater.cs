using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalConstantDeclarationEater : VariableDeclarationEater<ILocalConstantDeclaration>
    {
        public LocalConstantDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILocalConstantDeclaration variableDeclaration)
        {
            snapshot.Add(ExpressionKind.Stub, variableDeclaration);
        }
    }
}