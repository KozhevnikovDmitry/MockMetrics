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

        public override Metrics Eat(ISnapshot snapshot, IRegularParameterDeclaration variableDeclaration)
        {
            var result = Metrics.Create(Scope.Local, Variable.Data);
            snapshot.AddVariable(variableDeclaration, result);
            return result;
        }
    }
}