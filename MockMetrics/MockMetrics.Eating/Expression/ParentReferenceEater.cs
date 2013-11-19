using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public interface IParentReferenceEater
    {
        ExpressionKind Eat(ISnapshot snapshot, IInvocationExpression expression);
    }

    public class ParentReferenceEater : IParentReferenceEater, ICSharpNodeEater
    {
        private readonly IEater _eater;

        public ParentReferenceEater([NotNull] IEater eater)
        {
            if (eater == null) 
                throw new ArgumentNullException("eater");

            _eater = eater;
        }

        public virtual ExpressionKind Eat([NotNull] ISnapshot snapshot, [NotNull] IInvocationExpression expression)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (expression == null) 
                throw new ArgumentNullException("expression");

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