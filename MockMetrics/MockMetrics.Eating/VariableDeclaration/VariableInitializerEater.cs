using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableInitializerEater
    {
        Metrics Eat(ISnapshot snapshot, IVariableInitializer initializer);
    }

    public class VariableInitializerEater : IVariableInitializerEater, ICSharpNodeEater
    {
        private readonly IEater _eater;
        private readonly IMetricHelper _metricHelper;

        public VariableInitializerEater(IEater eater, IMetricHelper metricHelper)
        {
            _eater = eater;
            _metricHelper = metricHelper;
        }

        public virtual Metrics Eat([NotNull] ISnapshot snapshot, [NotNull] IVariableInitializer initializer)
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

                return Metrics.Create(Scope.Local, VarType.Library, Aim.Data);
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

        private Metrics EatResults(ISnapshot snapshot, ICSharpExpression initialExpression)
        {
            var resultMetrics = _eater.Eat(snapshot, initialExpression);
            resultMetrics.Scope = Scope.Local;
            return resultMetrics;
        }
    }
}