using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class UnsafeCodeFixedPointerEater : VariableDeclarationEater<IUnsafeCodeFixedPointerDeclaration>
    {
        public UnsafeCodeFixedPointerEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IUnsafeCodeFixedPointerDeclaration variableDeclaration)
        {
            throw new NotImplementedException();
        }
    }
}