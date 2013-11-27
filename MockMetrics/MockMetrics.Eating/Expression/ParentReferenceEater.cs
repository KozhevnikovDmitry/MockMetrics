using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public interface IParentReferenceEater
    {
        VarType Eat(ISnapshot snapshot, IInvocationExpression expression, bool innerEat);
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

        public virtual VarType Eat([NotNull] ISnapshot snapshot, [NotNull] IInvocationExpression expression, bool innerEat)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (expression == null) 
                throw new ArgumentNullException("expression");

            if (expression.ExtensionQualifier == null)
            {
                return VarType.None;
            }

            var mc = expression.ExtensionQualifier.ManagedConvertible;
            if (mc is ExtensionArgumentInfo)
            {
                return _eater.Eat(snapshot, (mc as ExtensionArgumentInfo).Expression);
            }

            return VarType.None;
        }
    }
}