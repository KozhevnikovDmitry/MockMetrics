using System.ComponentModel;

namespace MockMetrics.Eating.Expression
{
    /// <summary>
    /// Kinds of variable within unit test
    /// </summary>
    public enum ExpressionKind
    {
        [Description("Expression that can initiate stub")]
        StubCandidate,

        [Description("Stub, primitive, array, parameter")]
        Stub,

        [Description("Return value of target call")]
        Result,

        [Description("Moq.Mock<T> object or object of type in tests assemby")]
        Mock,

        [Description("Object of type from referenced projects")]
        Target,

        [Description("Method call of target object")]
        TargetCall,

        [Description("Call of Assert class methods, Verify or VerifyAll Mock<T> methods")]
        Assert,

        [Description("Unused in metrics")]
        None
    }
}