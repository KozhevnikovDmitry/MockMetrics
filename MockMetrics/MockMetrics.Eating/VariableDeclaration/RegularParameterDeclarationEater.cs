using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class RegularParameterDeclarationEater : VariableDeclarationEater<IRegularParameterDeclaration>
    {
        public RegularParameterDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IRegularParameterDeclaration variableDeclaration)
        {
            snapshot.Add(ExpressionKind.Stub, variableDeclaration);
        }
    }
}