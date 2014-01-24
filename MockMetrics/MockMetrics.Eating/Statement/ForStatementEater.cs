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
            if (statement.Initializer.Declaration != null)
            {
                foreach (var declarator in statement.Initializer.Declaration.Declarators)
                {
                    Eater.Eat(snapshot, declarator);
                }
            }

            foreach (var initializer in statement.Initializer.Expressions)
            {
                Eater.Eat(snapshot, initializer);
            }

            Eater.Eat(snapshot, statement.Condition);

            foreach (var iterator in statement.Iterators.Expressions)
            {
                Eater.Eat(snapshot, iterator);
            }

            Eater.Eat(snapshot, statement.Body);
        }
    }
}
