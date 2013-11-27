using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalConstantDeclarationEater : VariableDeclarationEater<ILocalConstantDeclaration>
    {
        public LocalConstantDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override Metrics Eat(ISnapshot snapshot, ILocalConstantDeclaration variableDeclaration)
        {
            var metrics = Metrics.Create(Scope.Local, VarType.Library, Aim.Data);
            snapshot.AddVariable(variableDeclaration, metrics);
            return metrics;
        }
    }
}