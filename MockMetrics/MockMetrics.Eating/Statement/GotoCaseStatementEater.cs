using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class GotoCaseStatementEater : StatementEater<IGotoCaseStatement>
    {
        public GotoCaseStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IGotoCaseStatement statement)
        {
            Eater.Eat(snapshot, statement.ValueExpression);
        }
    }
}