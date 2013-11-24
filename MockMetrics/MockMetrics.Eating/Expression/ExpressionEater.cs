using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

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

        public ExpressionKind Eat([NotNull] ISnapshot snapshot, [NotNull] ICSharpExpression expression, bool innerEat)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (expression == null) 
                throw new ArgumentNullException("expression");

            try
            {
                if (expression is T)
                {
                    return Eat(snapshot, (T)expression, innerEat);
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

        public abstract ExpressionKind Eat(ISnapshot snapshot, T expression, bool innerEat);
    }
}