using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HaveBox;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.InitializerElement;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.QueryClause;
using MockMetrics.Eating.Statement;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating
{
    public interface IEater : ICSharpNodeEater
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

        private IInitializerElementEater GetEater(IInitializerElement initializerElement)
        {
            try
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
            catch (Exception ex)
            {
                throw new EatingException(string.Format("Fail to get eater fo initializer element of type [{0}]", initializerElement.GetType()), ex, this, initializerElement);
            }
        }

        private IVariableDeclarationEater GetEater(IVariableDeclaration variableDeclaration)
        {
            try
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
            catch (Exception ex)
            {
                throw new EatingException(string.Format("Fail to get eater for variable declaration of type [{0}]", variableDeclaration.GetType()), ex, this, variableDeclaration);
            }
        }

        [DebuggerStepThrough]
        public IExpressionEater GetEater(ICSharpExpression expression)
        {
            try
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
            catch (Exception ex)
            {
                throw new EatingException(string.Format("Fail to get eater for expression of type [{0}]", expression.GetType()), ex, this, expression);
            }
        }

        [DebuggerStepThrough]
        public IStatementEater GetEater(ICSharpStatement statement)
        {
            try
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
            catch (Exception ex)
            {
                throw new EatingException(string.Format("Fail to get eater for statement of type [{0}]", statement.GetType()), ex, this, statement);
            }
        }

        [DebuggerStepThrough]
        public IQueryClauseEater GetEater(IQueryClause queryClause)
        {
            try
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
            catch (Exception ex)
            {
                throw new EatingException(string.Format("Fail to get eater for query clause of type [{0}]", queryClause.GetType()), ex, this, queryClause);
            }
        }
    }
}