using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class StubVariableDeclarationEater : IVariableDeclarationEater
    {
        public StubVariableDeclarationEater()
        {
            NodeType = GetType();
        }

        public Variable Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration)
        {
            return Variable.None;
        }

        public Type NodeType { get; private set; }
    }
}