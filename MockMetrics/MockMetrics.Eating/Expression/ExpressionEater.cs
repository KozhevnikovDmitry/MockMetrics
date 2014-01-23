using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public abstract class ExpressionEater<T> : NodeEater<T>, IExpressionEater where T : ICSharpExpression
    {
        protected readonly IEater Eater;

        protected ExpressionEater([NotNull] IEater eater)
        {
            if (eater == null) 
                throw new ArgumentNullException("eater");

            Eater = eater;
        }

        public Type ExpressionType
        {
            get { return typeof(T); }
        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] ICSharpExpression expression)
        {
            return EatNode(snapshot, expression, (s, n) => Eat(s, n));
        }

        public abstract Variable Eat(ISnapshot snapshot, T expression);
    }
}