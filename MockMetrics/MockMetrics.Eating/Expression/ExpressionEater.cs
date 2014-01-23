using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public abstract class ExpressionEater<T> : NodeEater<T>, IExpressionEater where T : ICSharpExpression
    {
        protected ExpressionEater([NotNull] IEater eater)
            : base(eater)
        {

        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] ICSharpExpression expression)
        {
            return EatNode(snapshot, expression, (s, n) => Eat(s, n));
        }

        public abstract Variable Eat(ISnapshot snapshot, T expression);
    }
}