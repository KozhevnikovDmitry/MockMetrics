using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalVariableDeclarationEater : VariableDeclarationEater<ILocalVariableDeclaration>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;

        public LocalVariableDeclarationEater(IEater eater, IVariableInitializerEater variableInitializerEater)
            : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
        }

        public override void Eat(ISnapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            ExpressionKind kind = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);

            if (kind == ExpressionKind.StubCandidate)
            {
                snapshot.AddTreeNode(ExpressionKind.Stub, variableDeclaration);
                return;
            }

            if (kind == ExpressionKind.TargetCall || kind == ExpressionKind.Assert)
            {
                snapshot.AddTreeNode(ExpressionKind.Result, variableDeclaration);
                return;
            }

            snapshot.AddTreeNode(kind, variableDeclaration);
        }
    }
}
