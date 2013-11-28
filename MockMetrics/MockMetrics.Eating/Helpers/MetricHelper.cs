using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Helpers
{
    public class MetricHelper : IMetricHelper
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public MetricHelper([NotNull]EatExpressionHelper eatExpressionHelper)
        {
            if (eatExpressionHelper == null)
                throw new ArgumentNullException("eatExpressionHelper");

            _eatExpressionHelper = eatExpressionHelper;
        }

        public Metrics MetricsMerge(Metrics first, Metrics second)
        {
            if (first.VarType >= second.VarType)
            {
                first.VarType = second.VarType;
            }
            return first;
        }

        public Metrics MetricsForCasted([NotNull] ISnapshot snapshot, 
                                        Metrics valueMetrics,
                                        [NotNull] ITypeUsage typeUsage)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (typeUsage == null) 
                throw new ArgumentNullException("typeUsage");

            var castMetrics = MetricsForType(snapshot, typeUsage);
            return MetricsMerge(valueMetrics, castMetrics);
        }

        // TODO : implement!
        public Metrics MetricsForReference(Metrics parentMetrics)
        {
            return Metrics.Create();
            throw new NotImplementedException();
        }

        // TODO : implement!
        public Metrics MetricsForAssignee(Metrics sourceMetrics)
        {
            return Metrics.Create();
            throw new NotImplementedException();
        }

        public Metrics MetricsForType([NotNull] ISnapshot snapshot, 
                                      [NotNull] ITypeUsage typeUsage)
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
                var typeElement = _eatExpressionHelper.GetUserTypeUsageClass(userTypeUsage);
                return  Metrics.Create(GetVarType(snapshot, typeElement));
            }

            return Metrics.Create(VarType.Library);
        }

        public Metrics MetricsForType([NotNull] ISnapshot snapshot, 
                                      [NotNull] IType type)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (type == null) throw new ArgumentNullException("type");

            return Metrics.Create(GetVarType(snapshot, type), GetAim(snapshot, type));
        }

        private VarType GetVarType(ISnapshot snapshot, IType type)
        {
            var typeElement = _eatExpressionHelper.GetTypeClass(type);
            return GetVarType(snapshot, typeElement);
        }

        private VarType GetVarType(ISnapshot snapshot, ITypeElement typeElement)
        {
            if (snapshot.IsInTestScope(typeElement.Module.Name))
            {
                //TODO if type is interface or abstract class return stub/mock? enum struct delegate?
                return VarType.Target;
            }

            if (snapshot.IsInTestProject(typeElement.Module.Name))
            {
                return VarType.Internal;
            }

            if (typeElement.ToString().StartsWith("Moq.Mock"))
            {
                return VarType.Mock;
            }

            return VarType.Library;
        }

        private Aim GetAim(ISnapshot snapshot, IType type)
        {
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

        // TODO : implement!
        public Metrics AcceptorMetrics(Metrics sourceMetrics)
        {
            return Metrics.Create();
            throw new NotImplementedException();
        }

        // TODO : implement!
        public Metrics ChildMetric(Metrics sourceMetrics)
        {
            return Metrics.Create();
            throw new NotImplementedException();
        }
    }
}
