using System;
using System.Collections.Generic;
using System.Linq;
using HaveBox;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating
{
    public interface IEater
    {
        ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression expression);
        
        void Eat(ISnapshot snapshot, ICSharpStatement statement);

        void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration);
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

        public void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration == null)
                throw new ArgumentNullException("variableDeclaration");

            GetEater(variableDeclaration).Eat(snapshot, variableDeclaration);
        }

        private IVariableDeclarationEater GetEater(IVariableDeclaration variableDeclaration)
        {
            var eater =
              _container.GetInstance<IEnumerable<IVariableDeclarationEater>>()
                  .SingleOrDefault(t => t.VariableDecalrationType.IsInstanceOfType(variableDeclaration));

            if (eater == null)
            {
                throw new NotSupportedException();
            }

            return eater;
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