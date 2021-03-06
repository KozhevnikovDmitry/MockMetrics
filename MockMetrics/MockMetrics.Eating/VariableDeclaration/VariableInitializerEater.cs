using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableInitializerEater
    {
        ExpressionKind Eat(ISnapshot snapshot, IVariableInitializer initializer);
    }

    public class VariableInitializerEater : IVariableInitializerEater, ICSharpNodeEater
    {
        private readonly IEater _eater;

        public VariableInitializerEater(IEater eater)
        {
            _eater = eater;
        }

        public ExpressionKind Eat([NotNull] ISnapshot snapshot, [NotNull] IVariableInitializer initializer)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (initializer == null) 
                throw new ArgumentNullException("initializer");

            if (initializer is IArrayInitializer)
            {
                foreach (IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
                {
                    ExpressionKind kind = Eat(snapshot, variableInitializer);

                    // TODO : what if stubcandidate
                    snapshot.Add(kind, variableInitializer);
                }

                // TODO : array of target?
                return ExpressionKind.StubCandidate;
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
            ExpressionKind expressionKind = _eater.Eat(snapshot, initialExpression);

            if (expressionKind == ExpressionKind.StubCandidate)
            {
                return ExpressionKind.Stub;
            }

            if (expressionKind == ExpressionKind.TargetCall)
            {
                return ExpressionKind.Result;
            }

            return expressionKind;
        }
    }
}