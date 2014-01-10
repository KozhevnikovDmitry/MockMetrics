using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableInitializerEater
    {
        Variable Eat(ISnapshot snapshot, IVariableInitializer initializer);
    }

    public class VariableInitializerEater : IVariableInitializerEater, ICSharpNodeEater
    {
        private readonly IEater _eater;

        public VariableInitializerEater(IEater eater)
        {
            _eater = eater;
        }

        public virtual Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IVariableInitializer initializer)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (initializer == null) 
                throw new ArgumentNullException("initializer");

            if (initializer is IArrayInitializer)
            {
                foreach (IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
                {
                    Eat(snapshot, variableInitializer);
                }

                return Variable.Library;
            }

            ICSharpExpression initialExpression = null;

            if (initializer is IExpressionInitializer)
            {
                initialExpression = (initializer as IExpressionInitializer).Value;
            }

            if (initializer is IUnsafeCodeFixedPointerInitializer)
            {
                initialExpression = (initializer as IUnsafeCodeFixedPointerInitializer).Value;
            }

            if (initializer is IUnsafeCodeStackAllocInitializer)
            {
                initialExpression = (initializer as IUnsafeCodeStackAllocInitializer).DimExpr;
            }

            return _eater.Eat(snapshot, initialExpression);
        }
    }
}