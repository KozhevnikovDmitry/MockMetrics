using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableInitializerEater
    {
        ExpressionKind Eat(ISnapshot snapshot, IVariableInitializer initializer);
    }

    public class VariableInitializerEater : IVariableInitializerEater
    {
        private readonly IEater _eater;

        public VariableInitializerEater(IEater eater)
        {
            _eater = eater;
        }

        public ExpressionKind Eat(ISnapshot snapshot,
            IVariableInitializer initializer)
        {
            if (initializer is IExpressionInitializer)
            {
                return _eater.Eat(snapshot, (initializer as IExpressionInitializer).Value);
            }

            if (initializer is IArrayInitializer)
            {
                foreach (
                    IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
                {
                    ExpressionKind kind = Eat(snapshot, variableInitializer);

                    snapshot.AddTreeNode(kind, variableInitializer);
                }

                return ExpressionKind.Stub;
            }

            if (initializer is IUnsafeCodeFixedPointerInitializer)
            {
                return _eater.Eat(snapshot, (initializer as IUnsafeCodeFixedPointerInitializer).Value);
            }

            if (initializer is IUnsafeCodeStackAllocInitializer)
            {
                return _eater.Eat(snapshot, (initializer as IUnsafeCodeStackAllocInitializer).DimExpr);
            }

            throw new NotSupportedException();
        }
    }
}