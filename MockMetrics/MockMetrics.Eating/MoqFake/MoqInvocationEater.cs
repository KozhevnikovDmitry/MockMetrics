using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqFake
{
    public class MoqInvocationEater : IMoqInvocationEater, ICSharpNodeEater
    {
        private readonly IMockOfInvocationEater _mockOfInvocationEater;
        private readonly IMoqFakeOptionEater _moqFakeOptionEater;
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly IInvocationStuffEater _invocationStuffEater;

        public MoqInvocationEater([NotNull] IMockOfInvocationEater mockOfInvocationEater,
                                  [NotNull] IMoqFakeOptionEater moqFakeOptionEater,
                                  [NotNull] EatExpressionHelper eatExpressionHelper,
                                  [NotNull] IInvocationStuffEater invocationStuffEater)
        {
            if (mockOfInvocationEater == null) throw new ArgumentNullException("mockOfInvocationEater");
            if (moqFakeOptionEater == null) throw new ArgumentNullException("moqFakeOptionEater");
            if (invocationStuffEater == null) throw new ArgumentNullException("invocationStuffEater");

            _mockOfInvocationEater = mockOfInvocationEater;
            _moqFakeOptionEater = moqFakeOptionEater;
            _eatExpressionHelper = eatExpressionHelper;
            _invocationStuffEater = invocationStuffEater;
        }

        public Variable Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            var invokedName = _eatExpressionHelper.GetInvokedElementName(expression);
            var result = Variable.Library;

            if (invokedName.StartsWith("Method:Moq.Mock.Get"))
            {
                result = Variable.None;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Returns"))
            {
                result = Variable.None;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Callback"))
            {
                snapshot.AddFakeOption(expression, FakeOption.CallBack);
                result = Variable.None;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Throws"))
            {
                snapshot.AddFakeOption(expression, FakeOption.Exception);
                result = Variable.None;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.SetupGet") ||
                invokedName.StartsWith("Method:Moq.Mock.SetupSet") ||
                invokedName.StartsWith("Method:Moq.Mock.VerifyGet") ||
                invokedName.StartsWith("Method:Moq.Mock.VerifySet") ||
                invokedName.StartsWith("Method:Moq.Mock.SetupProperty") ||
                invokedName.StartsWith("Method:Moq.Mock.SetupProperty") ||
                invokedName.StartsWith("Method:Moq.Mock.SetupAllProperties"))
            {
                snapshot.AddFakeOption(expression, FakeOption.Property);
                result = Variable.None;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Setup"))
            {
                _moqFakeOptionEater.Eat(snapshot, expression);
                result = Variable.None;

            }

            if (invokedName.StartsWith("Method:Moq.Mock.Of"))
            {
                _moqFakeOptionEater.Eat(snapshot, expression);
                result = Variable.Stub;
            }

            if (invokedName.StartsWith("Method:Moq.It.IsAny") ||
                invokedName.StartsWith("Method:Moq.It.IsIn") ||
                invokedName.StartsWith("Method:Moq.It.IsInRange") ||
                invokedName.StartsWith("Method:Moq.It.IsNotIn") ||
                invokedName.StartsWith("Method:Moq.It.IsRegex") ||
                invokedName.StartsWith("Method:Moq.It.IsNotNull"))
            {
                result = Variable.Stub;
            }

            if (invokedName.StartsWith("Method:Moq.It.Is"))
            {
                _moqFakeOptionEater.Eat(snapshot, expression);
                result = Variable.Stub;
            }

            _invocationStuffEater.Eat(snapshot, expression);

            if (_eatExpressionHelper.IsStandaloneMoqStubExpression(expression))
            {
                snapshot.AddVariable(expression, result);
            }

            return result;
        }

        public bool ContainsFakeOptions(IInvocationExpression expression)
        {
            var invokedName = _eatExpressionHelper.GetInvokedElementName(expression);
            return invokedName.StartsWith("Method:Moq");
        }
    }
}