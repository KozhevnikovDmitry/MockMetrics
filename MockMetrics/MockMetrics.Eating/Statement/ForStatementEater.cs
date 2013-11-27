using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Statement
{
    public class ForStatementEater : StatementEater<IForStatement>
    {
        public ForStatementEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IForStatement statement)
        {
            Eater.Eat(snapshot, statement.Body);

            Eater.Eat(snapshot, statement.Condition);

            foreach (var initializer in statement.Initializer.Expressions)
            {
                Eater.Eat(snapshot, initializer);
            }

            foreach (var iterator in statement.Iterators.Expressions)
            {
                Eater.Eat(snapshot, iterator);
            }
        }
    }
}
