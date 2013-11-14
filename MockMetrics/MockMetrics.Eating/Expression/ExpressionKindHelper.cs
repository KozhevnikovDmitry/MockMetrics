namespace MockMetrics.Eating.Expression
{
    public class ExpressionKindHelper
    {
        public virtual ExpressionKind ValueOfKindAsTypeOfKind(ExpressionKind valueKind, ExpressionKind typeKind)
        {
            if (valueKind == ExpressionKind.TargetCall)
            {
                return ExpressionKind.Result;
            }

            if (valueKind == ExpressionKind.Result)
            {
                return ExpressionKind.Result;
            }

            if (valueKind == ExpressionKind.Mock)
            {
                return ExpressionKind.Mock;
            }

            if (valueKind == ExpressionKind.Target || typeKind == ExpressionKind.Target)
            {
                return ExpressionKind.Target;
            }

            return valueKind;
        }
    }
}