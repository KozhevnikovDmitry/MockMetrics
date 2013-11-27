using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class SwitchStatementEater : StatementEater<ISwitchStatement>
    {
        public SwitchStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ISwitchStatement statement)
        {
            Eater.Eat(snapshot, statement.Block);
            Eater.Eat(snapshot, statement.Condition);
        }
    }
}
