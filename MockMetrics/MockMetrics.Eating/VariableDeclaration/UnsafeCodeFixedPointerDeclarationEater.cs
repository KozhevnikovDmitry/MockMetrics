using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class UnsafeCodeFixedPointerDeclarationEater : VariableDeclarationEater<IUnsafeCodeFixedPointerDeclaration>
    {
        public UnsafeCodeFixedPointerDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IUnsafeCodeFixedPointerDeclaration variableDeclaration)
        {
            var varType = Eater.Eat(snapshot, variableDeclaration.Initial);
            snapshot.AddVariable(variableDeclaration, varType);
            return varType;
        }
    }
}
