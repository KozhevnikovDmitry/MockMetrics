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

        public override Metrics Eat(ISnapshot snapshot, ITypeofExpression expression)
        {
            snapshot.AddCall(expression, Metrics.Create(Call.Library));

            return Metrics.Create(Variable.Data);
        }
    }
}