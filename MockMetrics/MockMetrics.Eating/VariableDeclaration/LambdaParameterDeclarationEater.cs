using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LambdaParameterDeclarationEater : VariableDeclarationEater<ILambdaParameterDeclaration>
    {
        private readonly MetricHelper _metricHelper;

        public LambdaParameterDeclarationEater(IEater eater, MetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override VarType Eat(ISnapshot snapshot, ILambdaParameterDeclaration variableDeclaration)
        {
            var metrics = _metricHelper.VarTypeAndAim(snapshot, variableDeclaration.Type);
            snapshot.AddVariable(variableDeclaration, Scope.Local, metrics.First, metrics.Second);
            return metrics.Second;
        }
    }
}