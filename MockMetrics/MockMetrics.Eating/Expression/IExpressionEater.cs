using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public interface IExpressionEater : ICSharpNodeEater
    {
        ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression statement);

        Type ExpressionType { get; }
    }

    public interface IExpressionEater<T> : IExpressionEater where T : ICSharpExpression
    {
        ExpressionKind Eat(ISnapshot snapshot, T expression);
    }
}
