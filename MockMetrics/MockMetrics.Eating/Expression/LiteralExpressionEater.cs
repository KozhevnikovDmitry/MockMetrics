using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class LiteralExpressionEater : ExpressionEater<ICSharpLiteralExpression>
    {
        public LiteralExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override Metrics Eat(ISnapshot snapshot, ICSharpLiteralExpression expression)
        {
            var result = Metrics.Create(Scope.Local, VarType.Library, Aim.Data);
            snapshot.AddOperand(expression, result);
            return result;
        }
    }
}