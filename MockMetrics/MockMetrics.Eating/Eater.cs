using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HaveBox;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.InitializerElement;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using MockMetrics.Eating.Statement;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating
{
    public interface IEater
    {
        Variable Eat(ISnapshot snapshot, ICSharpExpression expression);
        
        void Eat(ISnapshot snapshot, ICSharpStatement statement);

        Variable Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration);

        Variable Eat(ISnapshot snapshot, IQueryClause queryClause);

        Variable Eat(ISnapshot snapshot, IInitializerElement initializerElement);

        List<ICSharpTreeNode> EatedNodes { get; }
    }

    public class Eater : IEater
    {
        private readonly IContainer _container;

        public List<ICSharpTreeNode> EatedNodes { get; private set; }

        public Eater(IContainer container)
        {
            EatedNodes = new List<ICSharpTreeNode>();
            _container = container;
        }

        public Variable Eat(ISnapshot snapshot, ICSharpExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            EatedNodes.Add(expression);

            return GetEater(expression).Eat(snapshot, expression);
        }

        public void Eat(ISnapshot snapshot, ICSharpStatement statement)
        {
            if (statement == null)
                throw new ArgumentNullException("statement");

            EatedNodes.Add(statement);

            GetEater(statement).Eat(snapshot, statement);
        }

        public Variable Eat(ISnapshot snapshot, IInitializerElement initializerElement)
        {
            if (initializerElement == null)
                throw new ArgumentNullException("statement");

            EatedNodes.Add(initializerElement);

            return GetEater(initializerElement).Eat(snapshot, initializerElement);
        }

        public Variable Eat(ISnapshot snapshot, IQueryClause queryClause)
        {
            if (queryClause == null)
                throw new ArgumentNullException("queryClause");

            EatedNodes.Add(queryClause);

            return GetEater(queryClause).Eat(snapshot, queryClause);
        }

        public Variable Eat(ISnapshot snapshot, IVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration == null)
                throw new ArgumentNullException("variableDeclaration");

            EatedNodes.Add(variableDeclaration);

            return GetEater(variableDeclaration).Eat(snapshot, variableDeclaration);
        }

        [DebuggerStepThrough]
        private IInitializerElementEater GetEater(IInitializerElement initializerElement)
        {
            var eater =
             _container.GetInstance<IEnumerable<IInitializerElementEater>>()
                 .SingleOrDefault(t => t.InitializerElementType.IsInstanceOfType(initializerElement));

            if (eater == null)
            {
                return new StubInitializerElementEater();
            }

            return eater;
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