﻿using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class RegularParameterDeclarationEater : VariableDeclarationEater<IRegularParameterDeclaration>
    {
        public RegularParameterDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IRegularParameterDeclaration variableDeclaration)
        {
            snapshot.AddTreeNode(ExpressionKind.Stub, variableDeclaration);
        }
    }
}