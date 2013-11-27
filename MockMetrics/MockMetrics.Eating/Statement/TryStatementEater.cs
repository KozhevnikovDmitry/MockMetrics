using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class TryStatementEater : StatementEater<ITryStatement>
    {
        public TryStatementEater(IEater eater) : base(eater)
        {

        }

        public override void Eat(ISnapshot snapshot, ITryStatement statement)
        {
            Eater.Eat(snapshot, statement.Try);

            foreach (var catchClause in statement.Catches)
            {
                Eater.Eat(snapshot, catchClause.Body);
                if (catchClause is ISpecificCatchClause)
                {
                    Eater.Eat(snapshot, (catchClause as ISpecificCatchClause).ExceptionDeclaration);
                }
            }

            if (statement.FinallyBlock != null)
            {
                Eater.Eat(snapshot, statement.FinallyBlock);
            }
        }
    }
}
