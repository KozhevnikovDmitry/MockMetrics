using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.DeclaredElements;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree.Query;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Helpers
{

    public interface IRefereceEatHelper : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IReferenceExpression expression);
        Variable GetParentedVarType(ISnapshot snapshot, IReferenceExpression expression);
        Variable ExecuteResult(Variable vartype, ISnapshot snapshot, IReferenceExpression expression);
        bool IsStandaloneMethodReference(IReferenceExpression expression);
    }

    public class RefereceEatHelper : IRefereceEatHelper
    {
        private readonly IEater _eater;
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public RefereceEatHelper(IEater eater, IMetricHelper metricHelper, EatExpressionHelper eatExpressionHelper)
        {
            _eater = eater;
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public Variable GetParentedVarType(ISnapshot snapshot, IReferenceExpression expression)
        {
            var parentedVarType = Variable.None;
            if (expression.QualifierExpression != null)
            {
                var parentMetrics = _eater.Eat(snapshot, expression.QualifierExpression);

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

        public Variable Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

            if (declaredElement is IVariableDeclaration)
            {
                return snapshot.GetVarMetrics(declaredElement as IVariableDeclaration);
            }

            if (declaredElement is IQueryVariable)
            {
                return Variable.Service;
            }

            if (declaredElement is IParameter)
            {
                return snapshot.GetVarMetrics(declaredElement as IParameter);
            }

            if (declaredElement is IProperty)
            {
                return _metricHelper.MetricsForType(snapshot, (declaredElement as IProperty).Type);
            }

            if (declaredElement is IAnonymousTypeProperty)
            {
                return _metricHelper.MetricsForType(snapshot, (declaredElement as IAnonymousTypeProperty).Type);
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

        // TODO : Cover by unit tests
        public Variable ExecuteResult(Variable vartype, ISnapshot snapshot, IReferenceExpression expression)
        {
            if (IsUltimateReference(expression))
            {
                if (IsAssignAcceptor(expression))
                {
                    snapshot.AddVariable(expression, vartype);
                }
            }

            if (IsInternalVariable(expression, snapshot))
            {
                snapshot.AddVariable(expression, vartype);
            }

            if (IsEnumMember(expression))
            {
                snapshot.AddVariable(expression, vartype);
            }

            return vartype;
        }

        private bool IsEnumMember(IReferenceExpression expression)
        {
            var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);
            var declaration = declaredElement.GetDeclarations().FirstOrDefault();

            return declaration is IEnumMemberDeclaration;
        }

        private bool IsInternalVariable(IReferenceExpression expression, ISnapshot snapshot)
        {
            var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);
            if (declaredElement is IProperty ||
                declaredElement is IField)
            {
                if ((declaredElement as ITypeMember).GetContainingType() ==
                    snapshot.UnitTest.DeclaredElement.GetContainingType())
                {
                    return true;
                }
            }

            return false;
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

        // TODO : Cover by unit tests
        public bool IsStandaloneMethodReference(IReferenceExpression expression)
        {
            if (_eatExpressionHelper.IsStandaloneExpression(expression))
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

                if (declaredElement is IMethod)
                {
                    return true;
                }
            }

            return false;
        }
    }
}