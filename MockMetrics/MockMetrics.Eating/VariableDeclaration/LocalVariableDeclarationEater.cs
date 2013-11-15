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

            // TODO : cover by unit test
            // TODO : what it stub candidate
            if (kind == ExpressionKind.TargetCall)
            {
                snapshot.AddTreeNode(kind, variableDeclaration);
            }

            snapshot.AddTreeNode(kind, variableDeclaration);
        }
    }
}
