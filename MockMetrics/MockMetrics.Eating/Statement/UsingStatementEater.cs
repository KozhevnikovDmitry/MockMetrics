﻿using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class UsingStatementEater : StatementEater<IUsingStatement>
    {
        public UsingStatementEater(IEater eater) : base(eater)
        {
            
        }

        public override void Eat(ISnapshot snapshot, IUsingStatement statement)
        {
            foreach (var localVariableDeclaration in statement.VariableDeclarations)
            {
                Eater.Eat(snapshot, localVariableDeclaration);
            }

            Eater.Eat(snapshot, statement.Body);
        }
    }
}
