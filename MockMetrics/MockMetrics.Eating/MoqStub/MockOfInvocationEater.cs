using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqStub
{
    public interface IMockOfInvocationEater
    {
        void Eat(ISnapshot snapshot, IInvocationExpression expression);
    }

    public class MockOfInvocationEater : IMockOfInvocationEater, ICSharpNodeEater
    {
        private readonly IMoqStubOptionsEater _moqStubOptionsEater;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public MockOfInvocationEater(IMoqStubOptionsEater moqStubOptionsEater, EatExpressionHelper eatExpressionHelper)
        {
            _moqStubOptionsEater = moqStubOptionsEater;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public void Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            if (_eatExpressionHelper.IsStandaloneMoqStubExpression(expression))
            {
                snapshot.AddVariable(expression, Variable.Mock);
            }

            var predicate = expression.Arguments.Select(t => t.Value).OfType<ILambdaExpression>().SingleOrDefault();
            if (predicate != null)
            {
                if (predicate.ParameterDeclarations.Count != 1)
                {
                    throw new MoqStubWrongSyntaxException("Parameters count of moq-stub predicate differs from 1 ", this, predicate);
                }

                if (predicate.BodyBlock != null)
                {
                    throw new MoqStubWrongSyntaxException("Moq-stub predicate has bodyblock instead of body expression", this, predicate);
                }

                if (predicate.BodyExpression != null)
                {
                    _moqStubOptionsEater.EatStubOptions(snapshot, predicate.BodyExpression);
                }
            }
        }
    }

}
