using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class CatchVariableDeclarationEater : VariableDeclarationEater<ICatchVariableDeclaration>
    {
        public CatchVariableDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, ICatchVariableDeclaration variableDeclaration)
        {
            snapshot.AddVariable(variableDeclaration, Scope.Local, Aim.Result, VarType.Stub);
            return VarType.Stub;
        }
    }
}
