using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class GotoStatementEater : StatementEater<IGotoStatement>
    {
        public GotoStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IGotoStatement statement)
        {
            
        }
    }
}
