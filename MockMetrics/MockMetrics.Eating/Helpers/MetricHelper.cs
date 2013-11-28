using System;
using JetBrains.Annotations;
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
        /// Change operand metrics <paramref name="valueMetrics"/> by metrics of casting type <paramref name="castMetrics"/>
        /// </summary>
        /// <param name="valueMetrics">Metrics of casted operand</param>
        /// <param name="castMetrics">Metrics for casting type</param>
        Metrics CastExpressionType(Metrics valueMetrics, Metrics castMetrics);

        /// <summary>
        /// Metrics for reference operand by metrics of its parent reference <paramref name="parentMetrics"/>
        /// </summary>
        /// <param name="parentMetrics">Metrics of parent reference</param>
        Metrics RefMetricsByParentMetrics(Metrics parentMetrics);

        /// <summary>
        /// Metrics for assigned operand by metrics of assignment source <paramref name="sourceMetrics"/>
        /// </summary>
        /// <param name="sourceMetrics">Metrics of assignment source</param>
        /// <returns></returns>
        Metrics MetricsOfAssignment(Metrics sourceMetrics);

        Metrics MetricCastType(ISnapshot snapshot, ITypeUsage typeUsage);

        Metrics MetricVariable(ISnapshot snapshot, IType type);
    }

    /// <summary>
    /// Helps to determine metrics values in different cases
    /// </summary>
    public class MetricHelper : IMetricHelper
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public MetricHelper([NotNull] EatExpressionHelper eatExpressionHelper)
        {
            if (eatExpressionHelper == null)
                throw new ArgumentNullException("eatExpressionHelper");

            _eatExpressionHelper = eatExpressionHelper;
        }

        /// <summary>
        /// Metrics for operand with type.
        /// </summary>
        /// <param name="snapshot">Context snapshot</param>
        /// <param name="type">Type of the operand</param>
        public Metrics MetricsForType(ISnapshot snapshot, IType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change operand metrics <paramref name="valueMetrics"/> by metrics of casting type <paramref name="castMetrics"/>
        /// </summary>
        /// <param name="valueMetrics">Metrics of casted operand</param>
        /// <param name="castMetrics">Metrics for casting type</param>
        public Metrics CastExpressionType(Metrics valueMetrics, Metrics castMetrics)
        {
            if (valueMetrics.VarType >= castMetrics.VarType)
            {
                valueMetrics.VarType = castMetrics.VarType;
            }
            return valueMetrics;
        }

        /// <summary>
        /// Metrics for reference operand by metrics of its parent reference <paramref name="parentMetrics"/>
        /// </summary>
        /// <param name="parentMetrics">Metrics of parent reference</param>
        public Metrics RefMetricsByParentMetrics(Metrics parentMetrics)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Metrics for assigned operand by metrics of assignment source <paramref name="sourceMetrics"/>
        /// </summary>
        /// <param name="sourceMetrics">Metrics of assignment source</param>
        /// <returns></returns>
        public Metrics MetricsOfAssignment(Metrics sourceMetrics)
        {
            throw new NotImplementedException();
        }

        public Metrics MetricCastType([NotNull] ISnapshot snapshot, [NotNull] ITypeUsage typeUsage)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            if (typeUsage == null)
                throw new ArgumentNullException("typeUsage");

            if (typeUsage is IDynamicTypeUsage)
            {
                return Metrics.Create(VarType.Library);
            }

            if (typeUsage is IPredefinedTypeUsage)
            {
                return Metrics.Create(VarType.Library);
            }

            if (typeUsage is IUserTypeUsage)
            {
                var userTypeUsage = typeUsage as IUserTypeUsage;
                var classType = _eatExpressionHelper.GetUserTypeUsageClass(userTypeUsage);

                if (snapshot.IsInTestScope(classType.Module.Name))
                {
                    return Metrics.Create(VarType.Target);
                }

                if (snapshot.IsInTestProject(classType.Module.Name))
                {
                    return Metrics.Create(VarType.Mock);
                }
            }

            return Metrics.Create(VarType.Library);
        }

        public Metrics MetricVariable([NotNull] ISnapshot snapshot, [NotNull] IType type)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (type == null) throw new ArgumentNullException("type");

            return Metrics.Create(VarTypeVaribale(snapshot, type), AimVariableType(snapshot, type));
        }

        private VarType VarTypeVaribale([NotNull] ISnapshot snapshot, [NotNull] IType type)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            if (type == null)
                throw new ArgumentNullException("type");

            var classType = _eatExpressionHelper.GetTypeClass(type);
            if (snapshot.IsInTestScope(classType.Module.Name))
            {
                //TODO if type is interface or abstract class return stub/mock? enum struct delegate?
                return VarType.Target;
            }

            if (snapshot.IsInTestProject(classType.Module.Name))
            {
                return VarType.Internal;
            }

            if (classType.ToString().StartsWith("Moq.Mock"))
            {
                return VarType.Mock;
            }

            return VarType.Library;
        }

        private Aim AimVariableType([NotNull] ISnapshot snapshot, [NotNull] IType type)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            if (type == null)
                throw new ArgumentNullException("type");

            var classType = _eatExpressionHelper.GetTypeClass(type);
            if (snapshot.IsInTestScope(classType.Module.Name))
            {
                return Aim.Tested;
            }

            if (snapshot.IsInTestProject(classType.Module.Name))
            {
                return Aim.Data;
            }

            if (classType.ToString().StartsWith("Moq.Mock"))
            {
                return Aim.Data;
            }

            if (classType.Module.Name.ToLower().StartsWith("nunit.framework") ||
                classType.Module.Name.ToLower().StartsWith("moq"))
            {
                return Aim.Service;
            }

            return Aim.Data;
        }
    }
}
