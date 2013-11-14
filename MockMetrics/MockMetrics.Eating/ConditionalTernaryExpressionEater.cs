using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating
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
            snapshot.AddTreeNode(conditionKind, expression.ConditionOperand);

            var thenKind = Eater.Eat(snapshot, expression.ThenResult);
            snapshot.AddTreeNode(thenKind, expression.ThenResult);

            var elseKind = Eater.Eat(snapshot, expression.ElseResult);
            snapshot.AddTreeNode(elseKind, expression.ElseResult);

            return ExpressionKind.StubCandidate;
        }
    }
}