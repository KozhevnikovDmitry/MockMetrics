using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

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

        public override Variable Eat(ISnapshot snapshot, IUnsafeCodeFixedPointerDeclaration variableDeclaration)
        {
            var varType = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);
            snapshot.AddVariable(variableDeclaration, varType);
            return varType;
        }
    }
}
