using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class ForeachStatementEater : StatementEater<IForeachStatement>
    {
        public ForeachStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IForeachStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            var kind = Eater.Eat(snapshot, statement.Collection);
            snapshot.AddTreeNode(kind, statement.Collection);

            snapshot.AddVariable(statement.CurrentReference);
        }
    }
}
