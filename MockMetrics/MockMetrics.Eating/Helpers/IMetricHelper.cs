using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Helpers
{
    /// <summary>
    /// Helps to determine metrics values in different cases
    /// </summary>
    public interface IMetricHelper
    {
        /// <summary>
        /// Metrics for operand with type.
        /// </summary>
        /// <param name="snapshot">Context snapshot</param>
        /// <param name="type">Type of the operand</param>
        Variable MetricsForType(ISnapshot snapshot, IType type);

        /// <summary>
        /// Metrics for operand with type usage that associated with operand (cast for example).
        /// </summary>
        /// <param name="snapshot">Context snapshot</param>
        /// <param name="typeUsage">Type usage associated with operand</param>
        Variable MetricsForType(ISnapshot snapshot, ITypeUsage typeUsage);

        /// <summary>
        /// Merge two metrics, choose the most heavy metrics
        /// Returns new metrics object
        /// </summary>
        Variable MetricsMerge(Variable first, Variable second);
        
        /// <summary>
        /// Change operand metrics <paramref name="valueMetrics"/> by casting type <paramref name="typeUsage"/>
        /// </summary>
        /// <param name="snapshot">Context snapshot</param>
        /// <param name="valueMetrics">Metrics of casted operand</param>
        /// <param name="typeUsage">Casting type usage</param>
        Variable MetricsForCasted(ISnapshot snapshot, Variable valueType, ITypeUsage typeUsage);
        
        ///// <summary>
        ///// Scope for type(class, struct, enum, etc.) 
        ///// </summary>
        ///// <remarks>
        ///// If type is current tests-class, it will be Internal.
        ///// If type is from test scope or tests project, it will be External.
        ///// In other cases it will be Library.
        ///// </remarks>
        //Scope GetTypeScope(ISnapshot snapshot, ITypeElement typeElement);

        ///// <summary>
        ///// Metrics for method invocation by its parent reference
        ///// </summary>
        //Metrics CallMetrics(ISnapshot snapshot, IMethod invokedMethod, Metrics parentMetrics);
    }
}