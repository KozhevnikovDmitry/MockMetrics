using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class StubExpressionEater : IExpressionEater
    {
        public StubExpressionEater()
        {
            ExpressionType = GetType();
        }

        public ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression statement, bool innerEat)
        {
            return ExpressionKind.None;
        }

        public Type ExpressionType { get; private set; }
    }
}