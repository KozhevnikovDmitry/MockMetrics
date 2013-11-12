using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LambdaParameterDeclarationEater : VariableDeclarationEater<ILambdaParameterDeclaration>
    {
        public LambdaParameterDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILambdaParameterDeclaration variableDeclaration)
        {
            snapshot.AddVariable(variableDeclaration);
        }
    }
}