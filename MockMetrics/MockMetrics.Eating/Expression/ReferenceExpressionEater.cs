using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.DeclaredElements;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly IMetricHelper _metricHelper;
        private readonly IRefereceEatHelper _refereceEatHelper;

        public ReferenceExpressionEater(IEater eater, IMetricHelper metricHelper, IRefereceEatHelper refereceEatHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
            _refereceEatHelper = refereceEatHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            Metrics currentMetrics = _refereceEatHelper.Eat(snapshot, expression); 

            if (expression.QualifierExpression != null)
            {
                Metrics parentMetrics = Eater.Eat(snapshot, expression.QualifierExpression);
                currentMetrics.Scope = parentMetrics.Scope;

                if (parentMetrics.Variable == Variable.Target ||
                    parentMetrics.Variable == Variable.Mock ||
                    parentMetrics.Variable == Variable.Result ||
                    parentMetrics.Call == Call.TargetCall ||
                    parentMetrics.Call == Call.Assert)
                {
                    currentMetrics.Variable = Variable.Result;
                } 
                
                if (parentMetrics.Variable == Variable.Data)
                {
                    currentMetrics.Variable = Variable.Data;
                } 
                
                if (parentMetrics.Call == Call.Library && 
                    (parentMetrics.Variable == Variable.Target ||
                    parentMetrics.Variable == Variable.Mock ||
                    parentMetrics.Variable == Variable.Result) )
                {
                    currentMetrics.Variable = Variable.Result;
                }

                if (parentMetrics.Variable == Variable.External)
                {
                    currentMetrics.Variable = Variable.External;
                }
            }
           
            snapshot.AddOperand(expression, currentMetrics);
            return currentMetrics;
        }
    }

    public interface IRefereceEatHelper : ICSharpNodeEater
    {
        Metrics Eat(ISnapshot snapshot, IReferenceExpression expression);
    }

    public class RefereceEatHelper : IRefereceEatHelper
    {
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public RefereceEatHelper(IMetricHelper metricHelper, EatExpressionHelper eatExpressionHelper)
        {
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public Metrics Eat(ISnapshot snapshot, IReferenceExpression expression)
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
                return Metrics.Create(Scope.Local, Variable.Data);
            }

            if (declaredElement is ITypeElement)
            {
                return Metrics.Create(_metricHelper.GetTypeScope(snapshot, declaredElement as ITypeElement), Variable.External);
            }

            if (declaredElement is IMethod)
            {
                return Metrics.Create(Scope.Internal);
            }

            if (declaredElement is IEvent)
            {
                return Metrics.Create(Scope.Internal, Variable.Data);
            }

            // TODO : check if current namespace
            if (declaredElement is INamespace)
            {
                return Metrics.Create(Scope.External);
            } 
            
            if (declaredElement is IAlias)
            {
                return Metrics.Create(Scope.External);
            }

            throw new UnexpectedReferenceTypeException(declaredElement, this, expression);
        }
    }
}