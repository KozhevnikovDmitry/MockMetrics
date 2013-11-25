using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.MoqStub
{
    public interface IMockOfInvocationEater
    {
        ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression, bool innerEat);
    }

    // TODO : cover by unit tests
    public class MockOfInvocationEater : IMockOfInvocationEater, ICSharpNodeEater
    {
        private readonly IMoqStubOptionsEater _moqStubOptionsEater;

        public MockOfInvocationEater(IMoqStubOptionsEater moqStubOptionsEater)
        {
            _moqStubOptionsEater = moqStubOptionsEater;
        }

        public ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression, bool innerEat)
        {
            if (innerEat)
            {
                snapshot.Add(ExpressionKind.Stub, expression);
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

            return ExpressionKind.Stub;
        }
    }

}
