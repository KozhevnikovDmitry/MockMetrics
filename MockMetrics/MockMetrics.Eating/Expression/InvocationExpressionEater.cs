using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly VarTypeHelper _varTypeHelper;
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMockOfInvocationEater _mockOfInvocationEater;

        public InvocationExpressionEater(IEater eater, 
                                         EatExpressionHelper expressionHelper, 
                                         VarTypeHelper varTypeHelper,
                                         IParentReferenceEater parentReferenceEater,
                                         IArgumentsEater argumentsEater,
                                         IMockOfInvocationEater mockOfInvocationEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _varTypeHelper = varTypeHelper;
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
                snapshot.Add(ExpressionKind.Assert, expression);
                return ExpressionKind.Assert;
            }

            // TODO: special eater for moq mock verify, that will eat with inner eating
            if (invokedName.StartsWith("Method:Moq.Mock.Verify"))
            {
                snapshot.Add(ExpressionKind.Assert, expression);
                return ExpressionKind.Assert;
            }

            var invoked = _expressionHelper.GetInvokedElement(expression);
            if (invoked is IMethod)
            {
                var invokedMethod = invoked as IMethod;
                if (snapshot.IsInTestScope(invokedMethod.Module.Name))
                {
                    snapshot.Add(ExpressionKind.TargetCall, expression);
                    return ExpressionKind.TargetCall;
                }
            }

            if (parentKind == ExpressionKind.None)
            {
                return ExpressionKind.StubCandidate;
            }

            var basedOnParentKind = _varTypeHelper.InvocationKindByParentReferenceKind(parentKind);
            if (basedOnParentKind == ExpressionKind.TargetCall)
            {
                snapshot.Add(ExpressionKind.TargetCall, expression);
                return ExpressionKind.TargetCall;
            }

            return basedOnParentKind;
        }
    }
}