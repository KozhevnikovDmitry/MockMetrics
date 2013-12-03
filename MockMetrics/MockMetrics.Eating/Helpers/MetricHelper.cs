using System;
using System.Linq;
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

        public Metrics MetricsMerge([NotNull] Metrics first, [NotNull] Metrics second)
        {
            if (first == null) 
                throw new ArgumentNullException("first");
            if (second == null) 
                throw new ArgumentNullException("second");

            Metrics result = Metrics.Create();

            result.VarType = first.VarType >= second.VarType ? first.VarType : second.VarType;
            result.Scope = first.Scope >= second.Scope ? first.Scope : second.Scope;
            result.Call = first.Call >= second.Call ? first.Call : second.Call;
            result.Aim = first.Aim >= second.Aim ? first.Aim : second.Aim;

            return result;
        }

        public Metrics VarTypeMerge([NotNull] Metrics first, [NotNull] Metrics second)
        {
            if (first == null) 
                throw new ArgumentNullException("first");
            if (second == null) 
                throw new ArgumentNullException("second");

            first.VarType = first.VarType >= second.VarType ? first.VarType : second.VarType;

            return first;
        }

        public Metrics MetricsForCasted([NotNull] ISnapshot snapshot, 
                                        [NotNull] Metrics valueMetrics,
                                        [NotNull] ITypeUsage typeUsage)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            if (valueMetrics == null) 
                throw new ArgumentNullException("valueMetrics");

            if (typeUsage == null)
                throw new ArgumentNullException("typeUsage");

            var castMetrics = MetricsForType(snapshot, typeUsage);
            return MetricsMerge(valueMetrics, castMetrics);
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
                return Metrics.Create(GetVarType(snapshot, typeElement));
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

        public Scope GetTypeScope(ISnapshot snapshot, ITypeElement typeElement)
        {
            if (typeElement.Methods.Contains(snapshot.UnitTest.DeclaredElement))
            {
                return Scope.Internal;
            }

            if (snapshot.IsInTestScope(typeElement.Module.Name) ||
                snapshot.IsInTestProject(typeElement.Module.Name))
            {
                return Scope.External;
            }

            return Scope.Local;
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

        public Metrics CallMetrics(ISnapshot snapshot, IMethod invokedMethod, Metrics parentMetrics)
        {
            var result = Metrics.Create(parentMetrics.Scope);

            if (snapshot.IsInTestScope(invokedMethod.Module.Name) || 
                parentMetrics.VarType == VarType.Target ||
                parentMetrics.VarType == VarType.Mock)
            {
                result.Call = Call.TargetCall;
                result.Aim = Aim.Result;
                return result;
            }

            if (snapshot.IsInTestProject(invokedMethod.Module.Name))
            {
                result.Call = Call.Service;
                result.Aim = MetricsForType(snapshot, invokedMethod.ReturnType).Aim;
                return result;
            }

            result.Call = Call.Library;
            if (parentMetrics.Call == Call.TargetCall)
            {
                result.Aim = Aim.Result;
                return result;
            }

            if (parentMetrics.Call == Call.Assert)
            {
                result.Aim = Aim.Result;
                return result;
            }
            return result;
        }
    }
}
