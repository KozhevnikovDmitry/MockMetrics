using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LambdaParameterDeclarationEater : VariableDeclarationEater<ILambdaParameterDeclaration>
    {
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public LambdaParameterDeclarationEater(IEater eater, 
                                              [NotNull] IMetricHelper metricHelper,
                                              [NotNull] EatExpressionHelper eatExpressionHelper)
            : base(eater)
        {
            if (metricHelper == null) throw new ArgumentNullException("metricHelper");
            if (eatExpressionHelper == null) throw new ArgumentNullException("eatExpressionHelper");

            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public override Variable Eat([NotNull] ISnapshot snapshot,
                                     [NotNull] ILambdaParameterDeclaration variableDeclaration)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (variableDeclaration == null) throw new ArgumentNullException("variableDeclaration");

            Variable result;

            if (_eatExpressionHelper.IsMoqFakeOptionParameter(variableDeclaration))
            {
                result = Variable.Service;
            }
            else
            {
                result = _metricHelper.MetricsForType(snapshot, variableDeclaration.Type);
            }
            snapshot.AddVariable(variableDeclaration, result);
            return result;
        }
    }
}