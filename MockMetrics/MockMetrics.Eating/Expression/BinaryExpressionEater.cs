using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class BinaryExpressionEater : ExpressionEater<IBinaryExpression>
    {
        public BinaryExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IBinaryExpression expression, bool innerEat)
        {
            var leftKind = Eater.Eat(snapshot, expression.LeftOperand, innerEat);
            snapshot.Add(leftKind, expression.LeftOperand);

            var rightKind = Eater.Eat(snapshot, expression.RightOperand, innerEat);
            snapshot.Add(rightKind, expression.RightOperand);

            if (leftKind == ExpressionKind.TargetCall || rightKind == ExpressionKind.TargetCall)
            {
                return ExpressionKind.Result;
            }

            if (leftKind == ExpressionKind.Target || rightKind == ExpressionKind.Target)
            {
                return ExpressionKind.Target;
            }

            if (leftKind == ExpressionKind.Result || rightKind == ExpressionKind.Result)
            {
                return ExpressionKind.Result;
            }

            return ExpressionKind.StubCandidate;
        }
    }
}
