using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

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
            Eater.Eat(snapshot, statement.Condition);
        }
    }
}
