using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ConditionalTernaryExpressionEater : ExpressionEater<IConditionalTernaryExpression>
    {
        public ConditionalTernaryExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IConditionalTernaryExpression expression, bool innerEat)
        {
            var conditionKind = Eater.Eat(snapshot, expression.ConditionOperand, innerEat);
            snapshot.Add(conditionKind, expression.ConditionOperand);

            var thenKind = Eater.Eat(snapshot, expression.ThenResult, innerEat);
            snapshot.Add(thenKind, expression.ThenResult);

            var elseKind = Eater.Eat(snapshot, expression.ElseResult, innerEat);
            snapshot.Add(elseKind, expression.ElseResult);

            return ExpressionKind.StubCandidate;
        }
    }
}