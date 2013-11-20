using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class UnsafeCodeFixedPointerDeclarationEater : VariableDeclarationEater<IUnsafeCodeFixedPointerDeclaration>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;

        public UnsafeCodeFixedPointerDeclarationEater(IEater eater, IVariableInitializerEater variableInitializerEater)
            : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
        }

        public override void Eat(ISnapshot snapshot, IUnsafeCodeFixedPointerDeclaration variableDeclaration)
        {
            ExpressionKind kind = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);
            snapshot.Add(kind, variableDeclaration);
        }
    }
}
