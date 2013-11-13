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
            if (initializer is IArrayInitializer)
            {
                foreach (IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
                {
                    ExpressionKind kind = Eat(snapshot, variableInitializer);
                    snapshot.AddTreeNode(kind, variableInitializer);
                }

                return ExpressionKind.Stub;
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

            return EatResults(snapshot, initialExpression);
        }

        private ExpressionKind EatResults(ISnapshot snapshot, ICSharpExpression initialExpression)
        {
            if (initialExpression == null)
            {
                throw new NotSupportedException();
            }

            ExpressionKind expressionKind = _eater.Eat(snapshot, initialExpression);
            if (expressionKind == ExpressionKind.TargetCall)
            {
                return ExpressionKind.Result;
            }

            return expressionKind;
        }
    }
}