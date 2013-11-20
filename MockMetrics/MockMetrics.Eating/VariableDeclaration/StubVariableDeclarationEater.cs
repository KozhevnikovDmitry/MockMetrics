using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class StubVariableDeclarationEater : IVariableDeclarationEater
    {
        public StubVariableDeclarationEater()
        {
            VariableDecalrationType = GetType();
        }

        public void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration)
        {
            
        }

        public Type VariableDecalrationType { get; private set; }
    }
}