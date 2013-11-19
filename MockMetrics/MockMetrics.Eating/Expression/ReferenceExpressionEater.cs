using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly ExpressionKindHelper _kindHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly ITypeEater _typeEater;

        public ReferenceExpressionEater(IEater eater, ExpressionKindHelper kindHelper, EatExpressionHelper eatExpressionHelper, ITypeEater typeEater)
            : base(eater)
        {
            _kindHelper = kindHelper;
            _eatExpressionHelper = eatExpressionHelper;
            _typeEater = typeEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            var parentKind = expression.QualifierExpression != null
                ? Eater.Eat(snapshot, expression.QualifierExpression)
                : ExpressionKind.None;

            if (parentKind == ExpressionKind.None)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

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
                    return ExpressionKind.Stub;
                }

                if (declaredElement is ILocalConstantDeclaration)
                {
                    return ExpressionKind.Stub;
                }

                if (declaredElement is ILocalVariable)
                {
                    return snapshot.GetVariableKind(declaredElement as ILocalVariable, _typeEater);
                }

                if (declaredElement is IClass)
                {
                    return ExpressionKind.None;
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