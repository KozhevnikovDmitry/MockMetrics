using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MoqFake;

namespace MockMetrics.Eating.Helpers
{
    // TODO : cover by unit tests
    public class MoqSyntaxHelper
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public MoqSyntaxHelper([NotNull] EatExpressionHelper eatExpressionHelper)
        {
            if (eatExpressionHelper == null) throw new ArgumentNullException("eatExpressionHelper");
            _eatExpressionHelper = eatExpressionHelper;
        }

        public virtual MoqInvoke? GetMoqInvokeType(IInvocationExpression expression)
        {
            var invokedName = _eatExpressionHelper.GetInvokedElementName(expression);

            if (invokedName.Equals("Method:Moq.Mock.Get(T mocked)"))
            {
                return MoqInvoke.Mock;
            }

            if (invokedName.Equals("Method:Moq.Language.IReturns`2.Returns(TResult value)"))
            {
                return MoqInvoke.None;
            }

            if (invokedName.Equals("Method:Moq.Language.ICallback`2.Callback(System.Action action)") ||
                invokedName.Equals("Method:Moq.Language.ICallback.Callback(System.Action action)"))
            {
                return MoqInvoke.FakeCallback;
            }

            if (invokedName.StartsWith("Method:Moq.Language.IThrows.Throws"))
            {
                return MoqInvoke.FakeException;
            }

            if (invokedName.StartsWith("Method:Moq.Mock`1.SetupGet") ||
                invokedName.StartsWith("Method:Moq.Mock`1.SetupSet") ||
                invokedName.StartsWith("Method:Moq.Mock`1.VerifyGet") ||
                invokedName.StartsWith("Method:Moq.Mock`1.VerifySet") ||
                invokedName.StartsWith("Method:Moq.Mock`1.SetupProperty") ||
                invokedName.StartsWith("Method:Moq.Mock`1.SetupAllProperties"))
            {
                return MoqInvoke.FakeProperty;
            }

            if (invokedName.StartsWith("Method:Moq.It.IsAny") ||
                invokedName.StartsWith("Method:Moq.It.IsIn") ||
                invokedName.StartsWith("Method:Moq.It.IsInRange") ||
                invokedName.StartsWith("Method:Moq.It.IsNotIn") ||
                invokedName.StartsWith("Method:Moq.It.IsRegex") ||
                invokedName.StartsWith("Method:Moq.It.IsNotNull"))
            {
                return MoqInvoke.FakeWithoutOptions;
            }

            if (invokedName.StartsWith("Method:Moq.Mock`1.Setup") ||
                invokedName.StartsWith("Method:Moq.Mock`1.Verify"))
            {
                return MoqInvoke.FakeWithOptions;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Of") ||
                invokedName.StartsWith("Method:Moq.It.Is"))
            {
                return MoqInvoke.StubWithOptions;
            }

            return MoqInvoke.None;
        }
    }
}