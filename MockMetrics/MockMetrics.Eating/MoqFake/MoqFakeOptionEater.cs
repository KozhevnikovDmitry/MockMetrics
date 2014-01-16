using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqFake
{
    public interface IMoqFakeOptionEater
    {
        void Eat([NotNull] ISnapshot snapshot, [NotNull] IInvocationExpression invocationExpression);
    }

    public class MoqFakeOptionEater : IMoqFakeOptionEater, ICSharpNodeEater
    {
        private readonly IEater _eater;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public MoqFakeOptionEater([NotNull] IEater eater, [NotNull] EatExpressionHelper eatExpressionHelper)
        {
            _eater = eater;
            _eatExpressionHelper = eatExpressionHelper;
            if (eater == null) throw new ArgumentNullException("eater");
            if (eatExpressionHelper == null) throw new ArgumentNullException("eatExpressionHelper");
        }

        public void Eat(ISnapshot snapshot, IInvocationExpression invocationExpression)
        {
            var fakeArgument = invocationExpression.Arguments.FirstOrDefault();

            if (fakeArgument == null)
            {
                return;
            }

            if (fakeArgument.Expression is ILambdaExpression)
            {
                if (!(fakeArgument.Expression as ILambdaExpression).ParameterDeclarations.IsSingle())
                {
                    throw new NotSingleMoqFakeOptionLambdaParameterException(this, fakeArgument);
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