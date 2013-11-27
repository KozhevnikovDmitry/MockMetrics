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

        public override VarType Eat(ISnapshot snapshot, ILocalConstantDeclaration variableDeclaration)
        {
            snapshot.AddVariable(variableDeclaration, Scope.Local, Aim.Data, VarType.Library);
            return VarType.Library;
        }
    }
}