using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqFake
{
    public class MoqInvocationEater : IMoqInvocationEater, ICSharpNodeEater
    {
        private readonly IMockOfInvocationEater _mockOfInvocationEater;
        private readonly IItIsInvocationEater _itIsInvocationEater;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public MoqInvocationEater([NotNull] IMockOfInvocationEater mockOfInvocationEater,
                                  [NotNull] IItIsInvocationEater itIsInvocationEater,
                                  [NotNull] EatExpressionHelper eatExpressionHelper)
        {
            if (mockOfInvocationEater == null) throw new ArgumentNullException("mockOfInvocationEater");
            if (itIsInvocationEater == null) throw new ArgumentNullException("itIsInvocationEater");

            _mockOfInvocationEater = mockOfInvocationEater;
            _itIsInvocationEater = itIsInvocationEater;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public Variable Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            var invokedName = _eatExpressionHelper.GetInvokedElementName(expression);

            if (invokedName.StartsWith("Method:Moq.Mock.Of"))
            {
                _mockOfInvocationEater.Eat(snapshot, expression);
                return Variable.Stub;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Get"))
            {
                // TODO : special
                return Variable.None;
            }

            if (invokedName.StartsWith("Method:Moq.It.Is"))
            {
                _itIsInvocationEater.Eat(snapshot, expression);
                return Variable.Stub;
            }

            return Variable.Library;
        }
    }
}