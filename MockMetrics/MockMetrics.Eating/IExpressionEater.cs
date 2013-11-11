using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public interface IExpressionEater
    {
        ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression statement);

        Type ExpressionType { get; }
    }

    public interface IExpressionEater<T> : IExpressionEater where T : ICSharpExpression
    {
        ExpressionKind Eat(ISnapshot snapshot, T expression);
    }

    public abstract class ExpressionEater<T> : IExpressionEater where T : ICSharpExpression
    {
        protected readonly IEater Eater;

        protected ExpressionEater(IEater eater)
        {
            Eater = eater;
        }

        public Type ExpressionType
        {
            get { return typeof(T); }
        }

        public ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression expression)
        {
            if (expression is T)
            {
                return Eat(snapshot, (T)expression);
            }

            throw new NotSupportedException();
        }

        public abstract ExpressionKind Eat(ISnapshot snapshot, T expression);
    }
}
