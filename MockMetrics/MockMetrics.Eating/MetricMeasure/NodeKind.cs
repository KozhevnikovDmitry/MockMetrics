namespace MockMetrics.Eating.MetricMeasure
{
    //can be find by DeclaredElement
    public enum Operand
    {
        Constant,
        Variable,
        Argument,
        Property,
        Event
    }

    //can be find by DeclaredElement
    public enum Scope
    {
        Parameter,
        Local,
        Global
    }

    //can be find from context
    public enum Aim
    {
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
        TargetCall,
        Library,
        Assert,
        Internal,
        External
    }

    public enum FakeOption
    {
        Method,
        Property,
        Event,
        CallBack
    }

    public interface MockMetrics
    {
        
    }
}
