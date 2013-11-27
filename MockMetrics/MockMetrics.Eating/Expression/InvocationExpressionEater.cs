using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly MetricHelper _metric;
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMockOfInvocationEater _mockOfInvocationEater;

        public InvocationExpressionEater(IEater eater, 
                                         EatExpressionHelper expressionHelper,
                                         MetricHelper metric,
                                         IParentReferenceEater parentReferenceEater,
                                         IArgumentsEater argumentsEater,
                                         IMockOfInvocationEater mockOfInvocationEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _metric = metric;
            _parentReferenceEater = parentReferenceEater;
            _argumentsEater = argumentsEater;
            _mockOfInvocationEater = mockOfInvocationEater;
        }

        public override VarType Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            var invokedName = _expressionHelper.GetInvokedElementName(expression);

            if (invokedName.StartsWith("Method:Moq.Mock.Of"))
            {
                _mockOfInvocationEater.Eat(snapshot, expression);
                return VarType.Stub;
            }

            _argumentsEater.Eat(snapshot, expression.Arguments);

            var parentKind = _parentReferenceEater.Eat(snapshot, expression);


            // TODO: special eater for nunit asserts, that will eat with inner eating
            if (invokedName.StartsWith("Method:NUnit.Framework.Assert"))
            {
                snapshot.AddCall(expression, Call.Assert);
                return ;
            }

            // TODO: special eater for moq mock verify, that will eat with inner eating
            if (invokedName.StartsWith("Method:Moq.Mock.Verify"))
            {
                snapshot.AddCall(expression, Call.Assert);
                return ;
            }

            var invoked = _expressionHelper.GetInvokedElement(expression);
            if (invoked is IMethod)
            {
                var invokedMethod = invoked as IMethod;
                if (snapshot.IsInTestScope(invokedMethod.Module.Name))
                {
                    snapshot.AddCall(expression, Call.TargetCall);
                    return ;
                }
            }

            if (parentKind == VarType.None)
            {
                return VarType.StubCandidate;
            }

            var basedOnParentKind = _metric.InvocationKindByParentReferenceKind(parentKind);
            if (basedOnParentKind == ExpressionKind.TargetCall)
            {
                snapshot.Add(ExpressionKind.TargetCall, expression);
                return ExpressionKind.TargetCall;
            }

            return basedOnParentKind;
        }
    }
}