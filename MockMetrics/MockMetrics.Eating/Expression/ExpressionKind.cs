namespace MockMetrics.Eating
{
    /// <summary>
    /// Kinds of variable within unit test
    /// </summary>
    public enum ExpressionKind
    {
        Stub,
        Result,
        Mock,
        Target,
        TargetCall,
        Assert,
        None
    }
}