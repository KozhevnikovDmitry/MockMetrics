using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalVariableDeclarationEater : VariableDeclarationEater<ILocalVariableDeclaration>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;
        private readonly ITypeEater _typeEater;

        public LocalVariableDeclarationEater(IEater eater, IVariableInitializerEater variableInitializerEater, ITypeEater typeEater)
            : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
            _typeEater = typeEater;
        }

        public override void Eat(ISnapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration.Initial == null)
            {
                var typeKind = _typeEater.EatVariableType(snapshot, variableDeclaration.Type);
                snapshot.Add(typeKind, variableDeclaration);
                return;
            }

            ExpressionKind kind = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);

            if (kind == ExpressionKind.StubCandidate)
            {
                snapshot.Add(ExpressionKind.Stub, variableDeclaration);
                return;
            }

            if (kind == ExpressionKind.TargetCall || kind == ExpressionKind.Assert)
            {
                snapshot.Add(ExpressionKind.Result, variableDeclaration);
                return;
            }

            snapshot.Add(kind, variableDeclaration);
        }
    }
}
