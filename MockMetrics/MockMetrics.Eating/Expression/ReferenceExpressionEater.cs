using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly MetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly ITypeHelper _typeHelper;

        public ReferenceExpressionEater(IEater eater, MetricHelper metricHelper, EatExpressionHelper eatExpressionHelper, ITypeHelper typeHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
            _typeHelper = typeHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            Metrics parentMetrics = expression.QualifierExpression != null
                ? Eater.Eat(snapshot, expression.QualifierExpression)
                : Metrics.Create(VarType.Internal);

            if (parentMetrics.VarType == VarType.Internal)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

                // TODO: declared element can be method: case: event += obj.Method;
                // TODO: declared element can be parameter
                // TODO: Property(Field) can be Stub, Mock or Target
                if (declaredElement is IProperty)
                {
                    return _typeHelper.MetricVariable(snapshot, (declaredElement as IProperty).Type);
                }

                if (declaredElement is IField)
                {
                    return _typeHelper.MetricVariable(snapshot, (declaredElement as IField).Type);
                }

                if (declaredElement is IEvent)
                {
                    return VarType.Stub;
                }

                if (declaredElement is ILocalConstantDeclaration)
                {
                    return VarType.Stub;
                }

                if (declaredElement is IVariableDeclaration)
                {
                    return snapshot.GetVarType(declaredElement as IVariableDeclaration);
                }

                if (declaredElement is IClass)
                {
                    return VarType.None;
                }

                throw new UnexpectedReferenceTypeException(declaredElement, this, expression);
            }
            else
            {
                return _metricHelper.ReferenceKindByParentReferenceKind(parentKind);
            }
        }
    }
}