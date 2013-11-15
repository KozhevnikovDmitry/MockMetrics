using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public interface IParentReferenceEater
    {
        ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression);
    }

    public class ParentReferenceEater : IParentReferenceEater
    {
        private readonly IEater _eater;

        public ParentReferenceEater(IEater eater)
        {
            _eater = eater;
        }

        public virtual ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            if (expression.ExtensionQualifier == null)
            {
                return ExpressionKind.None;
            }

            var mc = expression.ExtensionQualifier.ManagedConvertible;
            if (mc is ExtensionArgumentInfo)
            {
                return _eater.Eat(snapshot, (mc as ExtensionArgumentInfo).Expression);
            }

            return ExpressionKind.None;
        }
    }
}