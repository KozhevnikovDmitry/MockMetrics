using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class ThrowStatementEater : StatementEater<IThrowStatement>
    {
        public ThrowStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IThrowStatement statement)
        {
            if (statement.Exception != null)
                Eater.Eat(snapshot, statement.Exception);
        }
    }
}
