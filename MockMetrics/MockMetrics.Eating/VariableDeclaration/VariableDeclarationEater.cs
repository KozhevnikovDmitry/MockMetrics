using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public abstract class VariableDeclarationEater<T> : IVariableDeclarationEater<T> where T : IVariableDeclaration
    {
        protected readonly IEater Eater;

        protected VariableDeclarationEater(IEater eater)
        {
            Eater = eater;
        }

        public void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration is T)
            {
                Eat(snapshot, (T)variableDeclaration);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public abstract void Eat(ISnapshot snapshot, T variableDeclaration);

        public Type VariableDecalrationType
        {
            get { return typeof (T); }
        }
    }
}