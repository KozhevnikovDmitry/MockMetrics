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

        public override VarType Eat(ISnapshot snapshot, ICSharpLiteralExpression expression)
        {
            snapshot.AddOperand(expression, Scope.Local, Aim.Data, VarType.Library);
            return VarType.Library;
        }
    }
}