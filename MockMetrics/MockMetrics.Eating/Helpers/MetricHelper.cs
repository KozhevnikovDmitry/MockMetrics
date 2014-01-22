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

        public Variable MetricsMerge(Variable first, Variable second)
        {
            return first >= second ? first : second;
        }

        public Variable MetricsForCasted([NotNull] ISnapshot snapshot,
                                         Variable valuetype,
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

            if (typeElement.ToString().Equals("Class:Moq.Mock`1"))
            {
                return Variable.Mock;
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

        public Variable GetReturnVarType(IInvocationExpression expression, ISnapshot snapshot)
        {
            if (_eatExpressionHelper.IsInternalMethod(expression, snapshot))
            {
                var method = _eatExpressionHelper.GetInvokedElement(expression) as IMethod;
                return MetricsForType(snapshot, method.ReturnType);
            }

            return Variable.Service;
        }
    }
}
