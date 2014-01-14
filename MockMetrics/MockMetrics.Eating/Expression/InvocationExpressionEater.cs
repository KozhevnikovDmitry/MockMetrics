using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqStub;

namespace MockMetrics.Eating.Expression
{
    public class InvocationExpressionEater : ExpressionEater<IInvocationExpression>
    {
        private readonly EatExpressionHelper _expressionHelper;
        private readonly IMetricHelper _metricHelper;
        private readonly IParentReferenceEater _parentReferenceEater;
        private readonly IArgumentsEater _argumentsEater;
        private readonly IMockOfInvocationEater _mockOfInvocationEater;

        public InvocationExpressionEater(IEater eater, 
                                         EatExpressionHelper expressionHelper,
                                         IMetricHelper metricHelper,
                                         IParentReferenceEater parentReferenceEater,
                                         IArgumentsEater argumentsEater,
                                         IMockOfInvocationEater mockOfInvocationEater)
            : base(eater)
        {
            _expressionHelper = expressionHelper;
            _metricHelper = metricHelper;
            _parentReferenceEater = parentReferenceEater;
            _argumentsEater = argumentsEater;
            _mockOfInvocationEater = mockOfInvocationEater;
        }

        public override Variable Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            var invokedName = _expressionHelper.GetInvokedElementName(expression);

            if (invokedName.StartsWith("Method:Moq.Mock.Of"))
            {
                _mockOfInvocationEater.Eat(snapshot, expression);
                return Variable.Stub;
            }

            if (invokedName.StartsWith("Method:Moq.Mock.Get"))
            {
                // TODO : special eating for Mock settings
                return Variable.None;
            }

            if (invokedName.StartsWith("Method:Moq.It.Is"))
            {
                // TODO : special eating for It.Is stubs
                return Variable.Stub;
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