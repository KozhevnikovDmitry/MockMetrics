using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class UnaryOperatorExpressionEater : ExpressionEater<IUnaryOperatorExpression>
    {
        public UnaryOperatorExpressionEater(IEater eater) : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IUnaryOperatorExpression expression)
        {
            return Eater.Eat(snapshot, expression.Operand);
        }
    }
}