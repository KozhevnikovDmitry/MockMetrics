using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class StubVariableDeclarationEater : IVariableDeclarationEater
    {
        public StubVariableDeclarationEater()
        {
            VariableDecalrationType = GetType();
        }

        public Variable Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration)
        {
            return Variable.None;
        }

        public Type VariableDecalrationType { get; private set; }
    }
}