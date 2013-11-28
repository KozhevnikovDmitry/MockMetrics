﻿using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
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

        public override Metrics Eat(ISnapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration.Initial == null)
            {
                var typeMetric = _metricHelper.MetricVariable(snapshot, variableDeclaration.Type);
                var result = Metrics.Create(typeMetric, Scope.Local);
                snapshot.AddVariable(variableDeclaration, result);
                return result;
            }

            var metrics = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);
            snapshot.AddVariable(variableDeclaration, metrics);
            return metrics;
        }
    }
}
