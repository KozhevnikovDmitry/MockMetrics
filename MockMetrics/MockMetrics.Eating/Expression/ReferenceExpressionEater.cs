using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly IMetricHelper _metricHelper;
        private readonly EatExpressionHelper _eatExpressionHelper;

        public ReferenceExpressionEater(IEater eater, IMetricHelper metricHelper, EatExpressionHelper eatExpressionHelper)
            : base(eater)
        {
            _metricHelper = metricHelper;
            _eatExpressionHelper = eatExpressionHelper;
        }

        public override Metrics Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            Metrics parentMetrics = expression.QualifierExpression != null
                ? Eater.Eat(snapshot, expression.QualifierExpression)
                : Metrics.Create(VarType.None);

            if (parentMetrics.VarType == VarType.None)
            {
                var declaredElement = _eatExpressionHelper.GetReferenceElement(expression);

                // TODO: declared element can be method: case: event += obj.Method;
                // TODO: declared element can be parameter
                // TODO: Property(Field) can be Stub, Mock or Target
                if (declaredElement is IProperty)
                {
                    Metrics result = _metricHelper.MetricVariable(snapshot, (declaredElement as IProperty).Type);
                    result.Scope = Scope.Internal;
                }

                if (declaredElement is IField)
                {
                    Metrics result = _metricHelper.MetricVariable(snapshot, (declaredElement as IField).Type);
                    result.Scope = Scope.Internal;
                }

                if (declaredElement is IEvent)
                {
                    return Metrics.Create(Scope.Internal, VarType.Internal);
                }
                
                if (declaredElement is IVariableDeclaration)
                {
                    return snapshot.GetVarType(declaredElement as IVariableDeclaration);
                }
                
                if (declaredElement is IClass)
                {
                    // static members of class containing unit test are internal
                    return Metrics.Create(Scope.External);
                }

                throw new UnexpectedReferenceTypeException(declaredElement, this, expression);
            }
            
            return _metricHelper.RefMetricsByParentMetrics(parentMetrics);
        }
    }
}