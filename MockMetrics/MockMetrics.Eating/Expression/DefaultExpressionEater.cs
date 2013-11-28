﻿using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class DefaultExpressionEater : ExpressionEater<IDefaultExpression>
    {
        private readonly IMetricHelper _metricHelper;

        public DefaultExpressionEater(IEater eater, IMetricHelper metricHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IDefaultExpression expression)
        {
            return Metrics.Create(_metricHelper.MetricCastType(snapshot, expression.TypeName));
        }
    }
}
