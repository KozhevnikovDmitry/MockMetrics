using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ArrayCreationExpressionEater : ExpressionEater<IArrayCreationExpression>
    {
        public ArrayCreationExpressionEater(IEater eater)
            : base(eater)
        {
        }

        public override Variable Eat(ISnapshot snapshot, IArrayCreationExpression expression)
        {
            foreach (ICSharpExpression size in expression.Sizes)
            {
                if (size != null)
                    Eater.Eat(snapshot, size);
            }

            if (expression.ArrayInitializer != null)
            {
                return Eater.Eat(snapshot, expression.ArrayInitializer);
            }
            
            return Variable.Library;
        }
    }
}
