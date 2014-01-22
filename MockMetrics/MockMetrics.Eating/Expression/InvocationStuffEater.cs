using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public interface IInvocationStuffEater
    {
        Variable Eat(ISnapshot snapshot, IInvocationExpression expression);
    }

    public class InvocationStuffEater : IInvocationStuffEater, ICSharpNodeEater
    {
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMetricHelper _metricHelper;

        public InvocationStuffEater([NotNull] IParentReferenceEater parentReferenceEater,
                                    [NotNull] IArgumentsEater argumentsEater, 
                                    [NotNull] IMetricHelper metricHelper)
        {
            if (parentReferenceEater == null) throw new ArgumentNullException("parentReferenceEater");
            if (argumentsEater == null) throw new ArgumentNullException("argumentsEater");
            if (metricHelper == null) throw new ArgumentNullException("metricHelper");
            _parentReferenceEater = parentReferenceEater;
            _argumentsEater = argumentsEater;
            _metricHelper = metricHelper;
        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IInvocationExpression expression)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (expression == null) throw new ArgumentNullException("expression");

            _argumentsEater.Eat(snapshot, expression.Arguments);
            Variable parentVarType = Variable.None;
            if (expression.ExtensionQualifier != null)
            {
                parentVarType = _parentReferenceEater.Eat(snapshot, expression);
            }

            return ExecuteResult(expression, snapshot, parentVarType);

        }

        private Variable ExecuteResult(IInvocationExpression expression, ISnapshot snapshot, Variable parentVarType)
        {
            if (parentVarType == Variable.Library)
            {
                return Variable.Library;
            }

            return _metricHelper.GetReturnVarType(expression, snapshot);
        }
    }
}