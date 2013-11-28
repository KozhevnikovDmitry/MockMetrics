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
        Metrics MetricsForType(ISnapshot snapshot, IType type);

        /// <summary>
        /// Metrics for operand with type usage that associated with operand (cast for example).
        /// </summary>
        /// <param name="snapshot">Context snapshot</param>
        /// <param name="typeUsage">Type usage associated with operand</param>
        Metrics MetricsForType(ISnapshot snapshot, ITypeUsage typeUsage);

        /// <summary>
        /// Merge two metrics, choose the most heavy var type
        /// </summary>
        Metrics MetricsMerge(Metrics first, Metrics second);

        /// <summary>
        /// Change operand metrics <paramref name="valueMetrics"/> by casting type <paramref name="typeUsage"/>
        /// </summary>
        /// <param name="snapshot">Context snapshot</param>
        /// <param name="valueMetrics">Metrics of casted operand</param>
        /// <param name="typeUsage">Casting type usage</param>
        Metrics MetricsForCasted(ISnapshot snapshot, Metrics valueMetrics, ITypeUsage typeUsage);

        /// <summary>
        /// Metrics for reference operand by metrics of its parent reference <paramref name="parentMetrics"/>
        /// </summary>
        /// <param name="parentMetrics">Metrics of parent reference</param>
        Metrics MetricsForReference(Metrics parentMetrics);

        /// <summary>
        /// Metrics for assigned operand by metrics of assignment source <paramref name="sourceMetrics"/>
        /// </summary>
        /// <param name="sourceMetrics">Metrics of assignment source</param>
        /// <returns></returns>
        Metrics MetricsForAssignee(Metrics sourceMetrics);
    }
}