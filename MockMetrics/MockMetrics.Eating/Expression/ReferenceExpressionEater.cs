using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.DeclaredElements;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly IRefereceEatHelper _refereceEatHelper;

        public ReferenceExpressionEater(IEater eater, IRefereceEatHelper refereceEatHelper)
            : base(eater)
        {
            _refereceEatHelper = refereceEatHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            var parentedVarType = GetParentedVarType(snapshot, expression);
            var curVarType = _refereceEatHelper.Eat(snapshot, expression);

            if (parentedVarType == Variable.Library)
            {
                curVarType = Variable.Library;
            }

            return ExecuteResult(curVarType, snapshot, expression);
        }

        private Variable GetParentedVarType(ISnapshot snapshot, IReferenceExpression expression)
        {
            var parentedVarType = Variable.None;
            if (expression.QualifierExpression != null)
            {
                var parentMetrics = Eater.Eat(snapshot, expression.QualifierExpression);

                switch (parentMetrics)
                {
                    case Variable.None:
                        {
                            break;
                        }
                    case Variable.Library:
                        {
                            parentedVarType = Variable.Library;
                            break;
                        }
                    case Variable.Stub:
                        {
                            parentedVarType = Variable.Service;
                            break;
                        }
                    case Variable.Mock:
                        {
                            parentedVarType = Variable.Service;
                            break;
                        }
                    case Variable.Target:
                        {
                            parentedVarType = Variable.Service;
                            break;
                        }
                    case Variable.Service:
                        {
                            parentedVarType = Variable.Service;
                            break;
                        }
                }
            }
            return parentedVarType;
        }

        private Variable ExecuteResult(Variable vartype, ISnapshot snapshot, IReferenceExpression expression)
        {
            if (IsUltimateReference(expression))
            {
                if (IsAssignAcceptor(expression))
                {
                    snapshot.AddVariable(expression, vartype);
                }
            }

            return vartype;
        }

        private bool IsAssignAcceptor(IReferenceExpression referenceExpression)
        {
            if (referenceExpression.Parent is IAssignmentExpression)
            {
                return (referenceExpression.Parent as IAssignmentExpression).Dest == referenceExpression;
            }

            return false;
        }

        private bool IsUltimateReference(IReferenceExpression expression)
        {
            return expression.FirstChild == null;
        }
    }

    public interface IRefereceEatHelper : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IReferenceExpression expression);
    }

    public class RefereceEatHelper : IRefereceEatHelper
    {
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public RefereceEatHelper(IMetricHelper metricHelper, EatExpressionHelper eatExpressionHelper)
        {
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public Variable Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

            if (declaredElement is IVariableDeclaration)
            {
                return snapshot.GetVarMetrics(declaredElement as IVariableDeclaration);
            }

            if (declaredElement is IParameter)
            {
                return snapshot.GetVarMetrics(declaredElement as IParameter);
            }

            if (declaredElement is IProperty)
            {
                return _metricHelper.MetricsForType(snapshot, (declaredElement as IProperty).Type);
            }

            if (declaredElement is IField)
            {
                return _metricHelper.MetricsForType(snapshot, (declaredElement as IField).Type);
            }

            if (declaredElement is IEnum)
            {
                return Variable.Library;
            }

            if (declaredElement is ITypeElement)
            {
                return _metricHelper.MetricForTypeReferece(snapshot, declaredElement as ITypeElement);
            }

            if (declaredElement is IMethod)
            {
                return Variable.Service;
            }

            if (declaredElement is IEvent)
            {
                return Variable.Service;
            }

            if (declaredElement is INamespace)
            {
                return Variable.None;
            }

            if (declaredElement is IAlias)
            {
                return Variable.None;
            }

            throw new UnexpectedReferenceTypeException(declaredElement, this, expression);
        }
    }
}