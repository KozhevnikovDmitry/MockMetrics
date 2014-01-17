using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public class CollectionElementInitializerEater : InitializerElementEater<ICollectionElementInitializer>
    {
        private readonly IArgumentsEater _argumentsEater;

        public CollectionElementInitializerEater([NotNull] IEater eater, [NotNull] IArgumentsEater argumentsEater) : base(eater)
        {
            if (argumentsEater == null) throw new ArgumentNullException("argumentsEater");
            _argumentsEater = argumentsEater;
        }

        public override Variable Eat([NotNull] ISnapshot snapshot, [NotNull] ICollectionElementInitializer initializer)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (initializer == null) throw new ArgumentNullException("initializer");

            _argumentsEater.Eat(snapshot, initializer.Arguments);

            return Variable.None;
        }
    }
}
