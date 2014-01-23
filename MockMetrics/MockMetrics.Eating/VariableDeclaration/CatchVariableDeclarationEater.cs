using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class CatchVariableDeclarationEater : VariableDeclarationEater<ICatchVariableDeclaration>
    {
        public CatchVariableDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, ICatchVariableDeclaration variableDeclaration)
        {
            snapshot.AddVariable(variableDeclaration, Variable.Library);
            return Variable.Library;
        }
    }
}
