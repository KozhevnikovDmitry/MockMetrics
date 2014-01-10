using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class RegularParameterDeclarationEater : VariableDeclarationEater<IRegularParameterDeclaration>
    {
        public RegularParameterDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IRegularParameterDeclaration variableDeclaration)
        {
            snapshot.AddVariable(variableDeclaration, Variable.Library);
            return Variable.Library;
        }
    }
}