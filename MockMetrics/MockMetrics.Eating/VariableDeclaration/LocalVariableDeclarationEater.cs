using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalVariableDeclarationEater : VariableDeclarationEater<ILocalVariableDeclaration>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;
        private readonly IMetricHelper _metricHelper;

        public LocalVariableDeclarationEater(IEater eater, 
                                             IVariableInitializerEater variableInitializerEater,
                                             IMetricHelper metricHelper)
            : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
            _metricHelper = metricHelper;
        }

        public override Variable Eat(ISnapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration.Initial == null)
            {
                var varType = _metricHelper.MetricsForType(snapshot, variableDeclaration.Type);
                snapshot.AddVariable(variableDeclaration, varType);
                return varType;
            }

            var variable = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);
            snapshot.AddVariable(variableDeclaration, variable);
            return variable;
        }
    }
}
