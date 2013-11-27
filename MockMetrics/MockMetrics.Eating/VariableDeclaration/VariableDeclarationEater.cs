using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public abstract class VariableDeclarationEater<T> : IVariableDeclarationEater<T> where T : IVariableDeclaration
    {
        protected readonly IEater Eater;

        protected VariableDeclarationEater([NotNull] IEater eater)
        {
            if (eater == null) 
                throw new ArgumentNullException("eater");

            Eater = eater;
        }

        public VarType Eat([NotNull] ISnapshot snapshot, [NotNull] IVariableDeclaration variableDeclaration)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (variableDeclaration == null) 
                throw new ArgumentNullException("variableDeclaration");

            try
            {
                if (variableDeclaration is T)
                {
                    return Eat(snapshot, (T)variableDeclaration);
                }
                else
                {
                    throw new UnexpectedTypeOfNodeToEatException(typeof(T), this, variableDeclaration);
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EatingException("Unexpected exception", ex, this, variableDeclaration);
            }
        }

        public abstract VarType Eat(ISnapshot snapshot, T variableDeclaration);

        public Type VariableDecalrationType
        {
            get { return typeof (T); }
        }
    }
}