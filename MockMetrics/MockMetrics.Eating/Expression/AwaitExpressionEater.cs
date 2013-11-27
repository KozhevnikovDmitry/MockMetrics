using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class AwaitExpressionEater : ExpressionEater<IAwaitExpression>
    {
        public AwaitExpressionEater(IEater eater) : base(eater)
        {
        }

        public override Metrics Eat(ISnapshot snapshot, IAwaitExpression expression)
        {
            return Eater.Eat(snapshot, expression.Task);
        }
    }
}