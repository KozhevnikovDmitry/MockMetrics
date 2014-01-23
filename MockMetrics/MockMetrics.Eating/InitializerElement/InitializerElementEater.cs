using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public abstract class InitializerElementEater<T> : NodeEater<T>, IInitializerElementEater<T> where T : IInitializerElement
    {
        protected readonly IEater Eater;

        protected InitializerElementEater([NotNull] IEater eater)
        {
            if (eater == null) 
                throw new ArgumentNullException("eater");

            Eater = eater;
        }

        public Type InitializerElementType
        {
            get { return typeof(T); }
        }

        public Variable Eat([NotNull] ISnapshot snapshot, [NotNull] IInitializerElement initializer)
        {
            return EatNode(snapshot, initializer, (s, n) => Eat(s, n));
        }

        public abstract Variable Eat(ISnapshot snapshot, T initializer);
    }
}