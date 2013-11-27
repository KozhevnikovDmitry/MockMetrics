namespace MockMetrics.Eating.MetricMeasure
{
    public struct Metrics
    {
        public static Metrics Create(VarType varType)
        {
            return new Metrics
            {
                VarType = varType
            };
        }

        public static Metrics Create(Aim aim, VarType varType)
        {
            return new Metrics
            {
                VarType = varType,
                Aim = aim
            };
        }

        public static Metrics Create(Scope scope, Aim aim, VarType varType)
        {
            return new Metrics
            {
                VarType = varType,
                Aim = aim,
                Scope = scope
            };
        }

        public static Metrics Create(Call call)
        {
            return new Metrics
            {
                Call = call
            };
        }

        public Scope Scope { get; set; }
        //public Operand Operand { get; set; }
        public Aim Aim { get; set; }
        public VarType VarType { get; set; }
        public Call Call { get; set; }

        public bool IsCallMetrics
        {
            get
            {
                return Call != Call.None;
            }
        }
    }

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
        Parameter,
        Local,
        Global
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
