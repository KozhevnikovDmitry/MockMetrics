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

        public override Variable Eat(ISnapshot snapshot, ICSharpLiteralExpression expression)
        {
            var result = Variable.Library;
            snapshot.AddVariable(expression, result);
            return result;
        }
    }
}