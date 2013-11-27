using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly VarTypeHelper _kindHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly ITypeEater _typeEater;

        public ReferenceExpressionEater(IEater eater, VarTypeHelper kindHelper, EatExpressionHelper eatExpressionHelper, ITypeEater typeEater)
            : base(eater)
        {
            _kindHelper = kindHelper;
            _eatExpressionHelper = eatExpressionHelper;
            _typeEater = typeEater;
        }

        public override VarType Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            var parentKind = expression.QualifierExpression != null
                ? Eater.Eat(snapshot, expression.QualifierExpression)
                : VarType.None;

            if (parentKind == VarType.None)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

                // TODO: declared element can be method: case: event += obj.Method;
                // TODO: declared element can be parameter
                // TODO: Property(Field) can be Stub, Mock or Target
                if (declaredElement is IProperty)
                {
                    return _typeEater.EatVariableType(snapshot, (declaredElement as IProperty).Type);
                }

                if (declaredElement is IField)
                {
                    return _typeEater.EatVariableType(snapshot, (declaredElement as IField).Type);
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
                return _kindHelper.ReferenceKindByParentReferenceKind(parentKind);
            }
        }
    }
}