using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly IMetricHelper _metricHelper;
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMockOfInvocationEater _mockOfInvocationEater;

        public InvocationExpressionEater(IEater eater, 
                                         EatExpressionHelper expressionHelper,
                                         IMetricHelper metricHelper,
                                         IParentReferenceEater parentReferenceEater,
                                         IArgumentsEater argumentsEater,
                                         IMockOfInvocationEater mockOfInvocationEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _metricHelper = metricHelper;
            _parentReferenceEater = parentReferenceEater;
            _argumentsEater = argumentsEater;
            _mockOfInvocationEater = mockOfInvocationEater;
        }

        public override Variable Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            var invokedName = _expressionHelper.GetInvokedElementName(expression);

            if (invokedName.StartsWith("Method:Moq.Mock.Of"))
            {
                _mockOfInvocationEater.Eat(snapshot, expression);
                return Metrics.Create(Scope.Local, Call.Library, Variable.Mock);
            }

            _argumentsEater.Eat(snapshot, expression.Arguments);

            var parentMetrics = _parentReferenceEater.Eat(snapshot, expression);

            // TODO: special eater for nunit asserts, that will eat with inner eating
            if (invokedName.StartsWith("Method:NUnit.Framework.Assert"))
            {
                var result = Metrics.Create(Scope.Local, Call.Assert, Variable.Result);
                snapshot.AddCall(expression, result); 
                return result;
            }

            // TODO: special eater for moq mock verify, that will eat with inner eating
            if (invokedName.StartsWith("Method:Moq.Mock.Verify"))
            {
                var result = Metrics.Create(parentMetrics.Scope, Call.Assert, Variable.Result);
                snapshot.AddCall(expression, result);
                return result;
            }

            var invoked = _expressionHelper.GetInvokedElement(expression);
            if (invoked is IMethod)
            {
                var invokedMethod = invoked as IMethod;
                var result = _metricHelper.CallMetrics(snapshot, invokedMethod, parentMetrics);
                snapshot.AddCall(expression, result);
                return result;
            }
            
            throw new UnexpectedInvokedElementTypeException(invoked, this, expression);
        }
    }
}