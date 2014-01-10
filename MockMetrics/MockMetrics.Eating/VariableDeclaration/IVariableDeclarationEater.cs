using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public interface IVariableDeclarationEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration);

        Type VariableDecalrationType { get; }
    }

    public interface IVariableDeclarationEater<T> : IVariableDeclarationEater where T : IVariableDeclaration
    {
        Variable Eat(ISnapshot snapshot, T variableDeclaration);
    }
}