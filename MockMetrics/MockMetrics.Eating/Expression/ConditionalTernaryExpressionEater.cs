using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ConditionalTernaryExpressionEater : ExpressionEater<IConditionalTernaryExpression>
    {
        public ConditionalTernaryExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, IConditionalTernaryExpression expression)
        {
            Eater.Eat(snapshot, expression.ConditionOperand);
            Eater.Eat(snapshot, expression.ThenResult);
            Eater.Eat(snapshot, expression.ElseResult);
            return VarType.Library;
        }
    }
}