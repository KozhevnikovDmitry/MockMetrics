using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
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

        public InvocationStuffEater([NotNull] IParentReferenceEater parentReferenceEater,
            [NotNull] IArgumentsEater argumentsEater)
        {
            if (parentReferenceEater == null) throw new ArgumentNullException("parentReferenceEater");
            if (argumentsEater == null) throw new ArgumentNullException("argumentsEater");
            _parentReferenceEater = parentReferenceEater;
            _argumentsEater = argumentsEater;
        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IInvocationExpression expression)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (expression == null) throw new ArgumentNullException("expression");

            _argumentsEater.Eat(snapshot, expression.Arguments);

            if (expression.ExtensionQualifier != null)
            {
                var parentVarType = _parentReferenceEater.Eat(snapshot, expression);
                return ExecuteResult(parentVarType);
            }

            return Variable.Service;

        }

        private Variable ExecuteResult(Variable parentVarType)
        {
            if (parentVarType == Variable.Library)
            {
                return Variable.Library;
            }

            return Variable.Service;
        }
    }
}