using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly MetricHelper _metricHelper;
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMockOfInvocationEater _mockOfInvocationEater;

        public InvocationExpressionEater(IEater eater, 
                                         EatExpressionHelper expressionHelper,
                                         MetricHelper metricHelper,
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

        public override Metrics Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            var invokedName = _expressionHelper.GetInvokedElementName(expression);

            if (invokedName.StartsWith("Method:Moq.Mock.Of"))
            {
                _mockOfInvocationEater.Eat(snapshot, expression);
                return Metrics.Create(Scope.Local, VarType.Stub, Aim.Data, Call.Library);
            }

            _argumentsEater.Eat(snapshot, expression.Arguments);

            var parentMetrics = _parentReferenceEater.Eat(snapshot, expression);

            // TODO: special eater for nunit asserts, that will eat with inner eating
            if (invokedName.StartsWith("Method:NUnit.Framework.Assert"))
            {
                var result = Metrics.Create(Scope.Local, Call.Assert);
                snapshot.AddCall(expression, result); 
                return result;
            }

            // TODO: special eater for moq mock verify, that will eat with inner eating
            if (invokedName.StartsWith("Method:Moq.Mock.Verify"))
            {
                var result = Metrics.Create(parentMetrics.Scope, Call.Assert);
                snapshot.AddCall(expression, result);
                return result;
            }

            var invoked = _expressionHelper.GetInvokedElement(expression);
            if (invoked is IMethod)
            {
                var result = Metrics.Create(parentMetrics.Scope);
                var invokedMethod = invoked as IMethod;
                if (snapshot.IsInTestScope(invokedMethod.Module.Name))
                {
                    result.Call = Call.TargetCall;
                    result.Aim = Aim.Result;
                    snapshot.AddCall(expression, result);
                    return result;
                }
            }

            var finalyResult = parentMetrics.ChildMetric();
            snapshot.AddCall(expression, finalyResult);
            return finalyResult;
        }
    }
}