using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class WhileStatementEater : StatementEater<IWhileStatement>
    {
        public WhileStatementEater(IEater eater) : base(eater)
        {

        }

        public override void Eat(ISnapshot snapshot, IWhileStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            var condKind = Eater.Eat(snapshot, statement.Condition);
            snapshot.AddTreeNode(condKind, statement.Condition);
        }
    }
}
