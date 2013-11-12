using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class LabelStatementEater : StatementEater<ILabelStatement>
    {
        public LabelStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILabelStatement statement)
        {
            snapshot.AddLabel(statement);
        }
    }
}
