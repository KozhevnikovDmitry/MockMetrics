using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public abstract class ExpressionEater<T> : IExpressionEater where T : ICSharpExpression
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

        public Metrics Eat([NotNull] ISnapshot snapshot, [NotNull] ICSharpExpression expression)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (expression == null) 
                throw new ArgumentNullException("expression");

            try
            {
                if (expression is T)
                {
                    return Eat(snapshot, (T)expression);
                }

                throw new UnexpectedTypeOfNodeToEatException(typeof(T), this, expression);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EatingException("Unexpected exception", ex, this, expression);
            }
        }

        public abstract Metrics Eat(ISnapshot snapshot, T expression);
    }
}