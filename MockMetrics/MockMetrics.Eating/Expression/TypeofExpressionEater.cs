using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class TypeofExpressionEater : ExpressionEater<ITypeofExpression>
    {
        public TypeofExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override VarType Eat(ISnapshot snapshot, ITypeofExpression expression)
        {
            return VarType.Library;
        }
    }
}