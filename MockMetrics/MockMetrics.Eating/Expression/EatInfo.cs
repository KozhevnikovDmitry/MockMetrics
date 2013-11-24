namespace MockMetrics.Eating.Expression
{
    public class EatInfo
    {
        public ExpressionKind Kind { get; set; }

        public bool IsNew { get; set; }

        public static implicit operator ExpressionKind(EatInfo eatInfo)
        {
            return eatInfo.Kind;
        }
    }
}
