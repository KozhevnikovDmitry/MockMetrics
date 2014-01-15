using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqFake;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMoqInvocationEater _moqInvocationEater;

        public InvocationExpressionEater(IEater eater, 
                                         EatExpressionHelper expressionHelper,
                                         IParentReferenceEater parentReferenceEater,
                                         IArgumentsEater argumentsEater,
                                         IMoqInvocationEater moqInvocationEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _parentReferenceEater = parentReferenceEater;
            _argumentsEater = argumentsEater;
            _moqInvocationEater = moqInvocationEater;
        }

        public override Variable Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            var invokedName = _expressionHelper.GetInvokedElementName(expression);

            if (invokedName.StartsWith("Method:Moq"))
            {
                return _moqInvocationEater.Eat(snapshot, expression);
            }

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