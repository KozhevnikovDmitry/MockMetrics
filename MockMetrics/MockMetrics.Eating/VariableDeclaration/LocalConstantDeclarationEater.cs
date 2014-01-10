using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalConstantDeclarationEater : VariableDeclarationEater<ILocalConstantDeclaration>
    {
        public LocalConstantDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, ILocalConstantDeclaration variableDeclaration)
        {
            snapshot.AddVariable(variableDeclaration, Variable.Library);
            return Variable.Library;
        }
    }
}