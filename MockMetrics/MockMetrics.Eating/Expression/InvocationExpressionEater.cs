using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqFake;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly IInvocationStuffEater _invocationStuffEater;
        private readonly IMoqInvocationEater _moqInvocationEater;

        public InvocationExpressionEater(IEater eater, 
                                         IInvocationStuffEater invocationStuffEater,
                                         IMoqInvocationEater moqInvocationEater)
            : base(eater)
        {
            _invocationStuffEater = invocationStuffEater;
            _moqInvocationEater = moqInvocationEater;
        }

        public override Variable Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            if (_moqInvocationEater.ContainsFakeOptions(expression))
            {
                return _moqInvocationEater.Eat(snapshot, expression);
            }

            return _invocationStuffEater.Eat(snapshot, expression);
        }
    }
}