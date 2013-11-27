using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableInitializerEater
    {
       Pair<Aim, VarType> Eat(ISnapshot snapshot, IVariableInitializer initializer);
    }

    public class VariableInitializerEater : IVariableInitializerEater, ICSharpNodeEater
    {
        private readonly IEater _eater;
        private readonly MetricHelper _metricHelper;

        public VariableInitializerEater(IEater eater, MetricHelper metricHelper)
        {
            _eater = eater;
            _metricHelper = metricHelper;
        }

        public Pair<Aim, VarType> Eat([NotNull] ISnapshot snapshot, [NotNull] IVariableInitializer initializer)
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

                return new Pair<Aim, VarType>(Aim.Data, VarType.Library);
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

        private Pair<Aim, VarType> EatResults(ISnapshot snapshot, ICSharpExpression initialExpression)
        {
            var varType = _eater.Eat(snapshot, initialExpression);
            var aim = _metricHelper.AimOfExpression(varType, initialExpression);
            return new Pair<Aim, VarType>(aim, varType);
        }
    }
}