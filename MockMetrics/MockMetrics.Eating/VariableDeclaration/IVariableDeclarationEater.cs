using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableDeclarationEater
    {
        void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration);

        Type VariableDecalrationType { get; }
    }

    public interface IVariableDeclarationEater<T> : IVariableDeclarationEater where T : IVariableDeclaration
    {
        void Eat(ISnapshot snapshot, T variableDeclaration);
    }
}