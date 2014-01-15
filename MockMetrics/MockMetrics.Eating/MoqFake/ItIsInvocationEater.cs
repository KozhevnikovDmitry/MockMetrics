using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqFake
{
    public interface IItIsInvocationEater
    {
        Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IInvocationExpression invocationExpression);
    }

    public class ItIsInvocationEater : IItIsInvocationEater, ICSharpNodeEater
    {
        private readonly IEater _eater;
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMoqFakeOptionEater _moqFakeOptionEater;

        public ItIsInvocationEater([NotNull] IEater eater, 
                                   [NotNull] EatExpressionHelper eatExpressionHelper,
                                   [NotNull] IArgumentsEater argumentsEater,
                                   [NotNull] IMoqFakeOptionEater moqFakeOptionEater)
        {
            if (eater == null) throw new ArgumentNullException("eater");
            if (eatExpressionHelper == null) throw new ArgumentNullException("eatExpressionHelper");
            if (argumentsEater == null) throw new ArgumentNullException("argumentsEater");
            if (moqFakeOptionEater == null) throw new ArgumentNullException("moqFakeOptionEater");

            _eater = eater;
            _eatExpressionHelper = eatExpressionHelper;
            _argumentsEater = argumentsEater;
            _moqFakeOptionEater = moqFakeOptionEater;
        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IInvocationExpression invocationExpression)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (invocationExpression == null) throw new ArgumentNullException("invocationExpression");

            var invokedName = _eatExpressionHelper.GetInvokedElementName(invocationExpression);

            _argumentsEater.Eat(snapshot, invocationExpression.Arguments);

            if (invokedName.StartsWith("Method:Moq.It.IsAny") ||
                invokedName.StartsWith("Method:Moq.It.IsIn") ||
                invokedName.StartsWith("Method:Moq.It.IsInRange") ||
                invokedName.StartsWith("Method:Moq.It.IsNotIn") ||
                invokedName.StartsWith("Method:Moq.It.IsRegex") ||
                invokedName.StartsWith("Method:Moq.It.IsNotNull"))
            {
                return Variable.Stub;
            }

            if (invokedName.StartsWith("Method:Moq.It.Is"))
            {
                _moqFakeOptionEater.Eat(snapshot, invocationExpression);
                return Variable.Stub;
            }

            throw new UnexpectedMoqItIsMethodNameException(invokedName, this, invocationExpression);
        }
    }
}