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

        public override Metrics Eat(ISnapshot snapshot, IIsExpression expression)
        {
            snapshot.AddOperand(expression.TypeOperand, Metrics.Create(Scope.Local, VarType.Library));
            Eater.Eat(snapshot, expression.Operand);
            return Metrics.Create(VarType.Library);
        }
    }
}
