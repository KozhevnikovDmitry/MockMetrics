namespace MockMetrics.Eating.MetricMeasure
{
    //can be find by DeclaredElement
    public enum Operand
    {
        None,
        Constant,
        Variable,
        Argument,
        Property,
        Event
    }

    //can be find by DeclaredElement
    public enum Scope
    {
        None,
        Local,
        Internal,
        External
    }

    //can be find from context
    public enum Aim
    {
        None,
        Data,
        Service,
        Result,
        Tested
    }

    public enum VarType
    {
        None,
        Library,
        Stub,
        Mock,
        Target,
        Internal,
        External
    }

    public enum Call
    {
        None,
        TargetCall,
        Library,
        Assert,
        Service
    }

    public enum FakeOption
    {
        Method,
        Property,
        Event,
        CallBack
    }
}
