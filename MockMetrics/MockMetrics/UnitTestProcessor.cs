using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using IExpressionStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IExpressionStatement;
using IForStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IForStatement;
using IIfStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IIfStatement;
using IObjectCreationExpression = JetBrains.ReSharper.Psi.CSharp.Tree.IObjectCreationExpression;
using ITryStatement = JetBrains.ReSharper.Psi.CSharp.Tree.ITryStatement;
using IUsingStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IUsingStatement;

namespace MockMetrics
{
    public class UnitTestProcessor
    {
        public Snapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            var snapshot = new Snapshot();
            EatBlockStatement(snapshot, unitTest.Body);
            return snapshot;
        }

        #region Eat Common Statements

        private void EatBlockStatement(Snapshot snapshot, IBlock block)
        {
            foreach (var statement in block.Statements.OfType<ICSharpStatement>())
            {
                EatStatement(snapshot, statement);
            }
        }

        private void EatStatement(Snapshot snapshot, ICSharpStatement statement)
        {
            if (statement is IDeclarationStatement)
            {
                EatDeclaration(snapshot, statement as IDeclarationStatement);
                return;
            }

            if (statement is IExpressionStatement)
            {
                EatExpressionStatement(snapshot, statement as IExpressionStatement);
                return;
            }

            if (statement is IBlock)
            {
                EatBlockStatement(snapshot, statement as IBlock);
                return;
            }

            if (statement is IIfStatement)
            {
                EatIfStatement(snapshot, statement as IIfStatement);
                return;
            }

            if (statement is IForStatement)
            {
                EatForStatement(snapshot, statement as IForStatement);
                return;
            }

            if (statement is IForeachStatement)
            {
                EatForeachStatement(snapshot, statement as IForeachStatement);
                return;
            }

            if (statement is IUsingStatement)
            {
                EatUsingStatement(snapshot, statement as IUsingStatement);
                return;
            }

            if (statement is IWhileStatement)
            {
                EatWhileStatement(snapshot, statement as IWhileStatement);
                return;
            }

            if (statement is IDoStatement)
            {
                EatDoStatement(snapshot, statement as IDoStatement);
                return;
            }

            if (statement is ITryStatement)
            {
                EatTryStatement(snapshot, statement as ITryStatement);
                return;
            }

        }

        #endregion
        

        #region Eat Declaration

        private void EatDeclaration(Snapshot snapshot, IDeclarationStatement declaration)
        {
            foreach (var localConstantDeclaration in declaration.ConstantDeclarationsEnumerable)
            {
                EatConstantDeclaration(snapshot, localConstantDeclaration);
            }

            foreach (var localVariableDeclaration in declaration.VariableDeclarationsEnumerable)
            {
                EatLocalVariableDeclaration(snapshot, localVariableDeclaration);
            }
        }

        private void EatLocalVariableDeclaration(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {
            if (localVariableDeclaration.Initial is IArrayInitializer)
            {
                snapshot.Stubs.Add(localVariableDeclaration);
                return;
            }

            if (localVariableDeclaration.Initial is IExpressionInitializer)
            {
                var initializer = localVariableDeclaration.Initial as IExpressionInitializer;
                if (initializer.Value is ICSharpLiteralExpression)
                {
                    snapshot.Stubs.Add(localVariableDeclaration);
                    return;
                }

                if (initializer.Value is IObjectCreationExpression)
                {
                    EatNewVariable(snapshot, localVariableDeclaration, initializer.Value as IObjectCreationExpression);

                }

                if (initializer.Value is IInvocationExpression)
                {
                    EatResultVariable(snapshot, localVariableDeclaration, initializer.Value as IInvocationExpression);

                }
            }
        }

        private void EatConstantDeclaration(Snapshot snapshot, ILocalConstantDeclaration localConstantDeclaration)
        {
            snapshot.Constants.Add(localConstantDeclaration);
        }


        private void EatNewVariable(Snapshot snapshot, 
                                    ILocalVariableDeclaration variableDeclaration,
                                    IObjectCreationExpression initializer)
        {
            var element = variableDeclaration.DeclaredElement;

            if (element.Type.Classify == TypeClassification.VALUE_TYPE)
            {
                snapshot.Stubs.Add(variableDeclaration);
                return;
            }

            if (element.Type.ToString().StartsWith("Moq.Mock`1[T -> "))
            {
                snapshot.Mocks.Add(variableDeclaration);
                EatMock(snapshot, variableDeclaration);
                return;
            }

            snapshot.TargetCandidates.Add(variableDeclaration);
            EatTargetCandidate(snapshot, variableDeclaration);
        }

        private void EatTargetCandidate(Snapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            var к = variableDeclaration;
        }

        private void EatMock(Snapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            var mock = variableDeclaration.DeclaredElement;
            
        }

        private void EatResultVariable(Snapshot snapshot,
                                       ILocalVariableDeclaration variableDeclaration,
                                       IInvocationExpression initializer)
        {
            var invokedMethod = initializer.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;
            if (invokedMethod.ToString().StartsWith("Method:Moq.Mock.Of()"))
            {
                snapshot.Stubs.Add(variableDeclaration);
                return;
            }

            snapshot.TargetCandidates.Add(variableDeclaration);
            EatTargetCandidate(snapshot, variableDeclaration);
        }

        #endregion


        #region Eat Expressions
        
        private void EatExpressionStatement(Snapshot snapshot, IExpressionStatement expression)
        {
            
            if (expression.Expression is IInvocationExpression)
            {
                var invocation = expression.Expression as IInvocationExpression;
                var invokedMethod = invocation.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;
                if (invokedMethod.ToString().StartsWith("Method:NUnit.Framework.Assert"))
                {
                    snapshot.Asserts.Add(expression);
                }
            }
        }

        private void EatExpression(Snapshot snapshot, ICSharpExpression expression)
        {

        }
        
        #endregion


        #region Eat CSharp Constructions

        private void EatIfStatement(Snapshot snapshot, IIfStatement ifStatement)
        {
            EatExpression(snapshot, ifStatement.Condition);
            EatStatement(snapshot, ifStatement.Then);

            if (ifStatement.ElseKeyword != null)
            {
                EatStatement(snapshot, ifStatement.Else);
            }
        }

        private void EatForStatement(Snapshot snapshot, IForStatement forStatement)
        {
            // TODO : eat for index init
            EatExpression(snapshot, forStatement.Condition);
            // TODO : eat for index iterate
            EatStatement(snapshot, forStatement.Body);
        }

        private void EatForeachStatement(Snapshot snapshot, IForeachStatement foreachStatement)
        {
            // TODO eat foreach currentReference as stub?
            EatExpression(snapshot, foreachStatement.Collection);
            EatStatement(snapshot, foreachStatement.Body);
        }

        private void EatUsingStatement(Snapshot snapshot, IUsingStatement usingStatement)
        {
            // TODO eat statement in using brackets: may be local variable declaration or variable assignment or just varibale name
            EatStatement(snapshot, usingStatement.Body);
        }

        private void EatWhileStatement(Snapshot snapshot, IWhileStatement whileStatement)
        {
            EatExpression(snapshot, whileStatement.Condition);
            EatStatement(snapshot, whileStatement.Body);
        }

        private void EatDoStatement(Snapshot snapshot, IDoStatement doStatement)
        {
            EatStatement(snapshot, doStatement.Body);
            EatExpression(snapshot, doStatement.Condition);
        }

        private void EatTryStatement(Snapshot snapshot, ITryStatement tryStatement)
        {
            EatBlockStatement(snapshot, tryStatement.Try);
            if (tryStatement.Catches.Any())
            {
                foreach (var catchClause in tryStatement.Catches)
                {
                    EatBlockStatement(snapshot, catchClause.Body);
                }
            }

            if (tryStatement.FinallyKeyword != null)
            {
                EatBlockStatement(snapshot, tryStatement.FinallyBlock);
            }
        }

        #endregion
    }
}
