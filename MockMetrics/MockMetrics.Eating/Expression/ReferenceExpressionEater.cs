using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public ReferenceExpressionEater(IEater eater, IMetricHelper metricHelper, EatExpressionHelper eatExpressionHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            Metrics parentMetrics = expression.QualifierExpression != null
                ? Eater.Eat(snapshot, expression.QualifierExpression)
                : Metrics.Create(VarType.None);

            if (parentMetrics.VarType == VarType.None)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

                if (declaredElement is IVariableDeclaration)
                {
                    return snapshot.GetVarMetrics(declaredElement as IVariableDeclaration);
                }

                if (declaredElement is IParameter)
                {
                    return snapshot.GetVarMetrics(declaredElement as IParameter);
                }

                if (declaredElement is IProperty)
                {
                    Metrics result = _metricHelper.MetricsForType(snapshot, (declaredElement as IProperty).Type);
                    result.Scope = Scope.Internal;
                    return result;
                }

                if (declaredElement is IField)
                {
                    Metrics result = _metricHelper.MetricsForType(snapshot, (declaredElement as IField).Type);
                    result.Scope = Scope.Internal;
                    return result;
                }

                if (declaredElement is IEnum)
                {
                    return Metrics.Create(Scope.Local, VarType.Library);
                }

                if (declaredElement is ITypeElement)
                {
                    return Metrics.Create(_metricHelper.GetTypeScope(snapshot, declaredElement as ITypeElement));
                }

                if (declaredElement is IMethod)
                {
                    return Metrics.Create(Scope.Internal);
                }

                if (declaredElement is IEvent)
                {
                    return Metrics.Create(Scope.Internal, VarType.Internal);
                }

                throw new UnexpectedReferenceTypeException(declaredElement, this, expression);
            }
            
            return _metricHelper.MetricsForReference(parentMetrics);
        }
    }
}