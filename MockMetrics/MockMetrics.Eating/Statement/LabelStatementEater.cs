using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class LabelStatementEater : StatementEater<ILabelStatement>
    {
        public LabelStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILabelStatement statement)
        {
            
        }
    }
}
