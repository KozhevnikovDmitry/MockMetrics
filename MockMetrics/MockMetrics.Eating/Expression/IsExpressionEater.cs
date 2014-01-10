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

        public override Variable Eat(ISnapshot snapshot, IIsExpression expression)
        {
            snapshot.AddVariable(expression.TypeOperand, Variable.Library);
            Eater.Eat(snapshot, expression.Operand);
            return Variable.Library;
        }
    }
}
