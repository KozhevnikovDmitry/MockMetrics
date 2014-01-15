using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
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

        public ItIsInvocationEater([NotNull] IEater eater, 
                                   [NotNull] EatExpressionHelper eatExpressionHelper,
                                   [NotNull] IArgumentsEater argumentsEater)
        {
            if (eater == null) throw new ArgumentNullException("eater");
            if (eatExpressionHelper == null) throw new ArgumentNullException("eatExpressionHelper");
            if (argumentsEater == null) throw new ArgumentNullException("argumentsEater");

            _eater = eater;
            _eatExpressionHelper = eatExpressionHelper;
            _argumentsEater = argumentsEater;
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
                if (!invocationExpression.Arguments.IsSingle())
                {
                    throw new NotSingleItIsInvacotaionArgumentException(this, invocationExpression);
                }

                EatFakeOption(snapshot, invocationExpression.Arguments.Single());
                return Variable.Stub;
            }

            throw new UnexpectedMoqItIsMethodNameException(invokedName, this, invocationExpression);
        }

        private void EatFakeOption(ISnapshot snapshot, ICSharpArgument fakeArgument)
        {
            if (fakeArgument.Expression is ILambdaExpression)
            {
                if (!(fakeArgument.Expression as ILambdaExpression).ParameterDeclarations.IsSingle())
                {
                    throw new NotSingleInIsOptionLambdaParameterException(this, fakeArgument);
                }

                var parameter = (fakeArgument.Expression as ILambdaExpression).ParameterDeclarations.Single();
                var methodFakes = _eater.EatedNodes.OfType<IInvocationExpression>()
                                        .Where(t => _eatExpressionHelper.GetParentReference(t) != null && 
                                                    _eatExpressionHelper.GetReferenceElement(_eatExpressionHelper.GetParentReference(t)).Equals(parameter));

                var propertyFakes = _eater.EatedNodes.OfType<IReferenceExpression>()
                                        .Where(t => _eatExpressionHelper.GetParentReference(t) != null && 
                                                    _eatExpressionHelper.GetReferenceElement(_eatExpressionHelper.GetParentReference(t)).Equals(parameter));

                foreach (var methodFake in methodFakes)
                {
                    snapshot.AddFakeOption(methodFake, FakeOption.Method);
                }

                foreach (var propertyFake in propertyFakes)
                {
                    snapshot.AddFakeOption(propertyFake, FakeOption.Property);
                }
            }
        }
    }
}