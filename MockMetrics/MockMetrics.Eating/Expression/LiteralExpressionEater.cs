using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class LiteralExpressionEater : ExpressionEater<ICSharpLiteralExpression>
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public LiteralExpressionEater(IEater eater, EatExpressionHelper eatExpressionHelper)
            : base(eater)
        {
            _eatExpressionHelper = eatExpressionHelper;
        }

        public override Variable Eat(ISnapshot snapshot, ICSharpLiteralExpression expression)
        {
            var result = Variable.Library;

            if (_eatExpressionHelper.IsStandaloneLiteralExpression(expression))
            {
                snapshot.AddVariable(expression, result);
            }
            return result;
        }
    }
}