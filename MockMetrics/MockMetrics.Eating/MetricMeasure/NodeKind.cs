namespace MockMetrics.Eating.MetricMeasure
{
    /// <summary>
    /// Reference operand type
    /// </summary>
    public enum Operand
    {
        None,
        Constant,
        Variable,
        Argument,
        Property,
        Event,
        Method,
        Type
    }

    /// <summary>
    /// Scope of operand 
    /// </summary>
    public enum Scope
    {
        None,

        /// <summary>
        /// Inside test
        /// </summary>
        Local,

        /// <summary>
        /// Inside test class
        /// </summary>
        Internal,

        /// <summary>
        /// Outside of test class
        /// </summary>
        External
    }
    
    public enum Variable
    {
        None,
        Service,
        Data,
        Result,
        Mock,
        Target
    }

    /// <summary>
    /// Type of invocation
    /// </summary>
    public enum Call
    {
        None,

        /// <summary>
        /// Tested call of target object
        /// </summary>
        TargetCall,

        /// <summary>
        /// Call of library methods
        /// </summary>
        Library,

        /// <summary>
        /// Assertations
        /// </summary>
        Assert,

        /// <summary>
        /// Calls of service methods of unit-test or mockery frameworks
        /// </summary>
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
