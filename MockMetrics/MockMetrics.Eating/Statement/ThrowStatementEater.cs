﻿using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class ThrowStatementEater : StatementEater<IThrowStatement>
    {
        public ThrowStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IThrowStatement statement)
        {
            var condKind = Eater.Eat(snapshot, statement.Exception);
            snapshot.Add(condKind, statement.Exception);
        }
    }
}
