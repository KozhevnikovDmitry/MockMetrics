using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.InitializerElement
{
    public abstract class InitializerElementEater<T> : IInitializerElementEater<T> where T : IInitializerElement
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
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (initializer == null) 
                throw new ArgumentNullException("initializer");

            try
            {
                if (initializer is T)
                {
                    return Eat(snapshot, (T)initializer);
                }

                throw new UnexpectedTypeOfNodeToEatException(typeof(T), this, initializer);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EatingException("Unexpected exception", ex, this, initializer);
            }
        }

        public abstract Variable Eat(ISnapshot snapshot, T initializer);
    }
}