namespace MockMetrics.Eating.MetricMeasure
{
    /// <summary>
    /// Reference opernad type
    /// </summary>
    public enum Operand
    {
        None,
        Constant,
        Variable,
        Argument,
        Property,
        Event,
        Method
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

    /// <summary>
    /// Aim of operand in test logic Arrange-Act-Assert
    /// </summary>
    public enum Aim
    {
        None,

        /// <summary>
        /// Data of test environment and configuration
        /// </summary>
        Data,

        /// <summary>
        /// Some service objects like mock-factories, that are not used in test logic directly
        /// </summary>
        Service,

        /// <summary>
        /// Asserted objects
        /// </summary>
        Result,

        /// <summary>
        /// Tested objects
        /// </summary>
        Tested
    }

    /// <summary>
    /// Operand or variable type
    /// </summary>
    public enum VarType
    {
        None,

        /// <summary>
        /// Some system or references library types
        /// </summary>
        Library,

        /// <summary>
        /// Moq-stubs
        /// </summary>
        Stub,

        /// <summary>
        /// Moq-mocks
        /// </summary>
        Mock,

        /// <summary>
        /// Object from test scope - user assemblies that are referenced by tests assemlby
        /// </summary>
        Target,

        /// <summary>
        /// Objects inside the test class
        /// </summary>
        Internal,

        /// <summary>
        /// Objects outside the test class
        /// </summary>
        External
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
