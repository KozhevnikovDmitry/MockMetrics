using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using MockMetrics.Eating.Exceptions;
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

        public Variable MetricsMerge(Variable first, Variable second)
        {
            return first >= second ? first : second;
        }

        public Variable MetricsForCasted([NotNull] ISnapshot snapshot,
                                        [NotNull] Variable valuetype,
                                        [NotNull] ITypeUsage typeUsage)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            if (typeUsage == null)
                throw new ArgumentNullException("typeUsage");

            var castMetrics = MetricsForType(snapshot, typeUsage);
            return MetricsMerge(valuetype, castMetrics);
        }

        public Variable MetricsForType([NotNull] ISnapshot snapshot,
                                       [NotNull] ITypeUsage typeUsage)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            if (typeUsage == null)
                throw new ArgumentNullException("typeUsage");

            if (typeUsage is IDynamicTypeUsage)
            {
                return Variable.Library;
            }

            if (typeUsage is IPredefinedTypeUsage)
            {
                return Variable.Library;
            }

            if (typeUsage is IUserTypeUsage)
            {
                var userTypeUsage = typeUsage as IUserTypeUsage;
                var typeElement = _eatExpressionHelper.GetUserTypeUsageClass(userTypeUsage);
                return GetVariable(snapshot, typeElement);
            }

            return Variable.Library;
        }

        public Variable MetricsForType([NotNull] ISnapshot snapshot,
                                      [NotNull] IType type)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (type == null) throw new ArgumentNullException("type");

            return GetVariable(snapshot, type);
        }

        private Variable GetVariable(ISnapshot snapshot, IType type)
        {
            if (type.Classify == TypeClassification.VALUE_TYPE)
            {
                return Variable.Library;
            }

            var typeElement = _eatExpressionHelper.GetTypeClass(type);
            return GetVariable(snapshot, typeElement);
        }

        private Variable GetVariable(ISnapshot snapshot, ITypeElement typeElement)
        {
            if (snapshot.IsInTestScope(typeElement.Module.Name))
            {
                if (typeElement is IInterface)
                {
                    return Variable.Stub;
                }

                if (typeElement is IEnum ||
                    typeElement is IStruct ||
                    typeElement is IDelegate)
                {
                    return Variable.Library;
                }
                
                // also ITypeParameter

                return Variable.Target;
            }

            if (snapshot.IsInTestProject(typeElement.Module.Name))
            {
                if (typeElement is IInterface)
                {
                    return Variable.Stub;
                }

                if (typeElement is IEnum ||
                    typeElement is IStruct ||
                    typeElement is IDelegate)
                {
                    return Variable.Library;
                }

                // also ITypeParameter

                return Variable.Mock;
            }

            if (typeElement.ToString().StartsWith("Moq.Mock"))
            {
                return Variable.Mock;
            }

            if (typeElement.Module.Name.ToLower().StartsWith("nunit.framework") ||
                typeElement.Module.Name.ToLower().StartsWith("moq"))
            {
                return Variable.Service;
            }

            return Variable.Library;
        }

        public Scope GetTypeScope([NotNull] ISnapshot snapshot, [NotNull] ITypeElement typeElement)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (typeElement == null) throw new ArgumentNullException("typeElement");
            if (snapshot.IsInTestProject(typeElement.Module.Name))
            {
                return Scope.Tested;
            }

            if (snapshot.IsInTestScope(typeElement.Module.Name))
            {
                return Scope.Test;
            }

            return Scope.Outside;
        }

        public Variable MetricForTypeReferece([NotNull] ISnapshot snapshot, [NotNull] ITypeElement typeElement)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (typeElement == null) throw new ArgumentNullException("typeElement");
            switch (GetTypeScope(snapshot, typeElement))
            {
                case Scope.Tested:
                    {
                        return Variable.Target;
                    }
                case Scope.Test:
                    {
                        return Variable.Service;
                    }
                case Scope.Outside:
                    {
                        return Variable.Library;
                    }
            }

            // TODO : more detailed exception
            throw new ArgumentException("typeElement");
        }

        //public Metrics CallMetrics(ISnapshot snapshot, IMethod invokedMethod, Metrics parentMetrics)
        //{
        //    var result = Metrics.Create(parentMetrics.Scope);

        //    if (snapshot.IsInTestScope(invokedMethod.Module.Name) ||
        //        parentMetrics.Variable == Variable.Target ||
        //        parentMetrics.Variable == Variable.Mock)
        //    {
        //        result.Call = Call.TargetCall;
        //        result.Variable = Variable.Result;
        //        return result;
        //    }

        //    if (snapshot.IsInTestProject(invokedMethod.Module.Name))
        //    {
        //        result.Call = Call.Service;
        //        result.Variable = MetricsForType(snapshot, invokedMethod.ReturnType).Variable;
        //        return result;
        //    }

        //    result.Call = Call.Library;
        //    if (parentMetrics.Variable == Variable.Data)
        //    {
        //        result.Variable = Variable.Data;
        //        return result;
        //    }

        //    if (parentMetrics.Call == Call.TargetCall)
        //    {
        //        result.Variable = Variable.Result;
        //        return result;
        //    }

        //    if (parentMetrics.Call == Call.Assert)
        //    {
        //        result.Variable = Variable.Result;
        //        return result;
        //    }
        //    return result;
        //}
    }
}
