using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableInitializerEater
    {
        VarType Eat(ISnapshot snapshot, IVariableInitializer initializer);
    }

    public class VariableInitializerEater : IVariableInitializerEater, ICSharpNodeEater
    {
        private readonly IEater _eater;

        public VariableInitializerEater(IEater eater)
        {
            _eater = eater;
        }

        public VarType Eat([NotNull] ISnapshot snapshot, [NotNull] IVariableInitializer initializer)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (initializer == null) 
                throw new ArgumentNullException("initializer");

            if (initializer is IArrayInitializer)
            {
                foreach (IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
                {
                    VarType varType = Eat(snapshot, variableInitializer);

                    // TODO : what if stubcandidate
                    snapshot.Add(varType, variableInitializer);
                }

                // TODO : array of target?
                return VarType.Library;
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

        private VarType EatResults(ISnapshot snapshot, ICSharpExpression initialExpression)
        {
            VarType varType = _eater.Eat(snapshot, initialExpression);

            if (varType == ExpressionKind.StubCandidate)
            {
                return ExpressionKind.Stub;
            }

            if (varType == ExpressionKind.TargetCall)
            {
                return ExpressionKind.Result;
            }

            return varType;
        }
    }
}