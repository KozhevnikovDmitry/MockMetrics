using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.Statement
{
    public abstract class StatementEater<T> : IStatementEater where T : ICSharpStatement
    {
        protected readonly IEater Eater;

        protected StatementEater([NotNull] IEater eater)
        {
            if (eater == null) 
                throw new ArgumentNullException("eater");
            Eater = eater;
        }

        public void Eat([NotNull] ISnapshot snapshot, [NotNull] ICSharpStatement statement)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (statement == null) 
                throw new ArgumentNullException("statement");

            try
            {
                if (statement is T)
                {
                    Eat(snapshot, (T) statement);
                }
                else
                {
                    throw new UnexpectedTypeOfNodeToEatException(typeof(T), this, statement);
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EatingException("Unexpected exception", ex, this, statement);
            }
        }

        public abstract void Eat(ISnapshot snapshot, T statement);

        public Type StatementType
        {
            get { return typeof (T); }
        }
    }
}