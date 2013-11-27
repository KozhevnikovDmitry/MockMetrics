using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class SwitchLabelStatementEater : StatementEater<ISwitchLabelStatement>
    {
        public SwitchLabelStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ISwitchLabelStatement statement)
        {
            Eater.Eat(snapshot, statement.ValueExpression);
        }
    }
}
