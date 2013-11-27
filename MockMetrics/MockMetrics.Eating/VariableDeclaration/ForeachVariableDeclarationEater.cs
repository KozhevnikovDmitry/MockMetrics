using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class ForeachVariableDeclarationEater : VariableDeclarationEater<IForeachVariableDeclaration>
    {
        private readonly MetricHelper _metricHelper;

        public ForeachVariableDeclarationEater(IEater eater, MetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override VarType Eat(ISnapshot snapshot, IForeachVariableDeclaration variableDeclaration)
        {
            var metrics = _metricHelper.VarTypeAndAim(snapshot, variableDeclaration.Type);
            snapshot.AddVariable(variableDeclaration, Scope.Local, metrics.First, metrics.Second);
            return metrics.Second;
        }
    }
}