using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class SwitchLabelStatementEater : StatementEater<ISwitchLabelStatement>
    {
        public SwitchLabelStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ISwitchLabelStatement statement)
        {
            var condKind = Eater.Eat(snapshot, statement.ValueExpression);
            snapshot.AddTreeNode(condKind, statement.ValueExpression);
        }
    }
}
