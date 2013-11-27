using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class IsExpressionEater : ExpressionEater<IIsExpression>
    {
        public IsExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IIsExpression expression)
        {
            Eater.Eat(snapshot, expression.Operand);
            return VarType.Library;
        }
    }
}
