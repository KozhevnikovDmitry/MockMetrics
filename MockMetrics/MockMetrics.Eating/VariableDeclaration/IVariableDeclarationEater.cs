using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableDeclarationEater : ICSharpNodeEater
    {
        void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration);

        Type VariableDecalrationType { get; }
    }

    public interface IVariableDeclarationEater<T> : IVariableDeclarationEater where T : IVariableDeclaration
    {
        void Eat(ISnapshot snapshot, T variableDeclaration);
    }
}