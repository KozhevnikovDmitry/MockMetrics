using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalVariableDeclarationEater : VariableDeclarationEater<ILocalVariableDeclaration>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;
        private readonly ITypeEater _typeEater;

        public LocalVariableDeclarationEater(IEater eater, 
                                             IVariableInitializerEater variableInitializerEater, 
                                             ITypeEater typeEater)
            : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
            _typeEater = typeEater;
        }

        public override VarType Eat(ISnapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration.Initial == null)
            {
                var varType = _typeEater.VarTypeVariableType(snapshot, variableDeclaration.Type);
                var aim = _typeEater.AimVariableType(snapshot, variableDeclaration.Type);
                snapshot.AddVariable(variableDeclaration, Scope.Local, aim, varType);
                return varType;
            }

            var metrics = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);
            snapshot.AddVariable(variableDeclaration, Scope.Local, metrics.First, metrics.Second);
            return metrics.Second;
        }
    }
}
