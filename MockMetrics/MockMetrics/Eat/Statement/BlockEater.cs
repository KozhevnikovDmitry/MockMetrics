using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eat.Statement
{
    public class BlockEater : IEater<IBlock>
    {
        public Snapshot Eat(Snapshot snapshot, IMethodDeclaration unitTest, IBlock block)
        {
            foreach (var statement in block.Statements)
            {
                
            }

            return snapshot;
        }
    }
}