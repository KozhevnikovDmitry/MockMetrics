using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class AnonymousMethodParameterDeclarationEater : VariableDeclarationEater<IAnonymousMethodParameterDeclaration>
    {
        public AnonymousMethodParameterDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IAnonymousMethodParameterDeclaration variableDeclaration)
        {
            snapshot.Add(variableDeclaration);
        }
    }
}