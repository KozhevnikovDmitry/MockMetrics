using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ConditionalTernaryExpressionEater : ExpressionEater<IConditionalTernaryExpression>
    {
        public ConditionalTernaryExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IConditionalTernaryExpression expression)
        {
            var conditionKind = Eater.Eat(snapshot, expression.ConditionOperand);
            snapshot.Add(conditionKind, expression.ConditionOperand);

            var thenKind = Eater.Eat(snapshot, expression.ThenResult);
            snapshot.Add(thenKind, expression.ThenResult);

            var elseKind = Eater.Eat(snapshot, expression.ElseResult);
            snapshot.Add(elseKind, expression.ElseResult);

            return ExpressionKind.StubCandidate;
        }
    }
}