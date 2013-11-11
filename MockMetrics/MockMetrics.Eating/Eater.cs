using System;
using System.Collections.Generic;
using System.Linq;
using HaveBox;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public interface IEater
    {
        ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression expression);
        void Eat(ISnapshot snapshot, ICSharpStatement statement);
    }

    public class Eater : IEater
    {
        private readonly IContainer _container;

        public Eater(IContainer container)
        {
            _container = container;
        }

        public ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return GetEater(expression).Eat(snapshot, expression);
        }

        public void Eat(ISnapshot snapshot, ICSharpStatement statement)
        {
            if (statement == null)
                throw new ArgumentNullException("statement");

            GetEater(statement).Eat(snapshot, statement);
        }

        public IExpressionEater GetEater(ICSharpExpression expression)
        {
            var eater =
              _container.GetInstance<IEnumerable<IExpressionEater>>()
                  .SingleOrDefault(t => t.ExpressionType.IsInstanceOfType(expression));

            if (eater == null)
            {
                throw new NotSupportedException();
            }

            return eater;
        }
        
        public IStatementEater GetEater(ICSharpStatement statement)
        {
            var eater =
                _container.GetInstance<IEnumerable<IStatementEater>>()
                    .SingleOrDefault(t => t.StatementType.IsInstanceOfType(statement));

            if (eater == null)
            {
                throw new NotSupportedException();
            }

            return eater;
        }
    }
}