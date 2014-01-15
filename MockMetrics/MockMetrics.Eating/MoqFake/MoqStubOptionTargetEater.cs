using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqFake
{
    public interface IMoqStubOptionTargetEater
    {
        FakeOption EatOption(ISnapshot snapshot, IInvocationExpression invocationExpression);

        FakeOption EatOption(ISnapshot snapshot, IReferenceExpression referenceExpression);
    }

    public class MoqStubOptionTargetEater : IMoqStubOptionTargetEater, ICSharpNodeEater
    {
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly IArgumentsEater _argumentsEater;

        public MoqStubOptionTargetEater(EatExpressionHelper eatExpressionHelper, IArgumentsEater argumentsEater)
        {
            _eatExpressionHelper = eatExpressionHelper;
            _argumentsEater = argumentsEater;
        }

        public virtual FakeOption EatOption(ISnapshot snapshot, IInvocationExpression invocationExpression)
        {
            _argumentsEater.Eat(snapshot, invocationExpression.Arguments);
            
            var parentReference = _eatExpressionHelper.GetInvocationReference(invocationExpression);
            if (parentReference == null)
            {
                throw new MoqStubWrongSyntaxException("Moq-stub invocation-option has not parent reference", this,
                    invocationExpression);
            }

            if (parentReference is IInvocationExpression)
            {
                return EatOption(snapshot, parentReference as IInvocationExpression);
            }

            if (parentReference is IReferenceExpression)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(parentReference as IReferenceExpression);
                if (declaredElement is ILambdaParameterDeclaration)
                {
                    return FakeOption.Method;
                }

                return EatOption(snapshot, parentReference as IReferenceExpression);
            }

            throw new MoqStubOptionTargetWrongTypeException(this, parentReference);
        }

        public virtual FakeOption EatOption(ISnapshot snapshot, IReferenceExpression referenceExpression)
        {
            if (referenceExpression.QualifierExpression == null)
            {
                throw new MoqStubWrongSyntaxException("Moq-stub property-option has not parent reference", this,
                    referenceExpression);
            }

            if (referenceExpression.QualifierExpression is IInvocationExpression)
            {
                return EatOption(snapshot, referenceExpression.QualifierExpression as IInvocationExpression);
            }

            if (referenceExpression.QualifierExpression is IReferenceExpression)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(referenceExpression.QualifierExpression as IReferenceExpression);
                if (declaredElement is ILambdaParameterDeclaration)
                {
                    return FakeOption.Property;
                }
                return EatOption(snapshot, referenceExpression.QualifierExpression as IReferenceExpression);
            }

            throw new MoqStubOptionTargetWrongTypeException(this, referenceExpression.QualifierExpression);
        }
    }
}