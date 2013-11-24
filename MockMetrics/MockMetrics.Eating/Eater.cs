using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HaveBox;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.QueryClause;
using MockMetrics.Eating.Statement;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating
{
    public interface IEater
    {
        ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression expression, bool innerEat = false);
        
        void Eat(ISnapshot snapshot, ICSharpStatement statement);

        void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration);

        ExpressionKind Eat(ISnapshot snapshot, IQueryClause queryClause);
    }

    public class Eater : IEater
    {
        private readonly IContainer _container;

        public Eater(IContainer container)
        {
            _container = container;
        }

        public ExpressionKind Eat(ISnapshot snapshot, ICSharpExpression expression, bool innerEat = false)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return GetEater(expression).Eat(snapshot, expression, innerEat);
        }

        public void Eat(ISnapshot snapshot, ICSharpStatement statement)
        {
            if (statement == null)
                throw new ArgumentNullException("statement");

            GetEater(statement).Eat(snapshot, statement);
        }

        public ExpressionKind Eat(ISnapshot snapshot, IQueryClause queryClause)
        {
            if (queryClause == null)
                throw new ArgumentNullException("queryClause");

            return GetEater(queryClause).Eat(snapshot, queryClause);
        }

        public void Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration == null)
                throw new ArgumentNullException("variableDeclaration");

            GetEater(variableDeclaration).Eat(snapshot, variableDeclaration);
        }
        
        [DebuggerStepThrough]
        private IVariableDeclarationEater GetEater(IVariableDeclaration variableDeclaration)
        {
            var eater =
              _container.GetInstance<IEnumerable<IVariableDeclarationEater>>()
                  .SingleOrDefault(t => t.VariableDecalrationType.IsInstanceOfType(variableDeclaration));

            if (eater == null)
            {
                return new StubVariableDeclarationEater();
            }

            return eater;
        }
        
        [DebuggerStepThrough]
        public IExpressionEater GetEater(ICSharpExpression expression)
        {
            var eater =
              _container.GetInstance<IEnumerable<IExpressionEater>>()
                  .SingleOrDefault(t => t.ExpressionType.IsInstanceOfType(expression));

            if (eater == null)
            {
                return new StubExpressionEater();
            }

            return eater;
        }

        [DebuggerStepThrough]
        public IStatementEater GetEater(ICSharpStatement statement)
        {
            var eater =
                _container.GetInstance<IEnumerable<IStatementEater>>()
                    .SingleOrDefault(t => t.StatementType.IsInstanceOfType(statement));

            if (eater == null)
            {
                return new StubStatementEater();
            }

            return eater;
        }

        [DebuggerStepThrough]
        public IQueryClauseEater GetEater(IQueryClause queryClause)
        {
            var eater =
                _container.GetInstance<IEnumerable<IQueryClauseEater>>()
                    .SingleOrDefault(t => t.QueryClauseType.IsInstanceOfType(queryClause));

            if (eater == null)
            {
                return new StubClauseEater();
            }

            return eater;
        }
    }
}