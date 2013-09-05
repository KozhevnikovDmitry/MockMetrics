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
            var snapshot = new Snapshot(unitTest);
            EatBlock(snapshot, unitTest.Body);
            return snapshot;
        }

        #region Eat Statements

        private void EatBlock(Snapshot snapshot, IBlock block)
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
                EatBlock(snapshot, statement as IBlock);
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
            EatBlock(snapshot, tryStatement.Try);
            if (tryStatement.Catches.Any())
            {
                foreach (var catchClause in tryStatement.Catches)
                {
                    EatBlock(snapshot, catchClause.Body);
                }
            }

            if (tryStatement.FinallyKeyword != null)
            {
                EatBlock(snapshot, tryStatement.FinallyBlock);
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

        private void EatConstantDeclaration(Snapshot snapshot, ILocalConstantDeclaration localConstantDeclaration)
        {
            snapshot.Constants.Add(localConstantDeclaration);
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
                EatExpressionVariable(snapshot, initializer.Value, localVariableDeclaration);
            }
        }

        private void EatExpressionVariable(Snapshot snapshot, ICSharpExpression initializerExpression, ILocalVariableDeclaration localVariableDeclaration)
        {
            if (initializerExpression is ICSharpLiteralExpression)
            {
                EatCSharpLiteralExpression(snapshot, localVariableDeclaration);
                return;
            }

            if (initializerExpression is IObjectCreationExpression)
            {
                EatObjectCreationExpression(snapshot, localVariableDeclaration);
                return;
            }

            if (initializerExpression is IInvocationExpression)
            {
                EatInvocationExpression(snapshot, localVariableDeclaration, initializerExpression as IInvocationExpression);
                return;
            }

            if (initializerExpression is IReferenceExpression)
            {
                EatReferenceExpression(snapshot, localVariableDeclaration);
                return;
            }

            if (initializerExpression is IEqualityExpression)
            {
                EatEqualityExpression(snapshot, localVariableDeclaration);
                return;
            }

            if (initializerExpression is IUnaryExpression)
            {
                EatUnaryExpression(snapshot, localVariableDeclaration);
                return;
            }

            if (initializerExpression is IBinaryExpression)
            {
                EatBinaryExpression(snapshot, localVariableDeclaration);
                return;
            }

            if (initializerExpression is IConditionalTernaryExpression)
            {
                EatConditionalTernaryExpression(snapshot, localVariableDeclaration);
                return;
            }
        }

        #region Eat Local Variable Initial Expression

        private void EatCSharpLiteralExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {
            snapshot.Stubs.Add(localVariableDeclaration);
        }

        private void EatObjectCreationExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {
            var element = localVariableDeclaration.DeclaredElement;

            if (element.Type.Classify == TypeClassification.VALUE_TYPE)
            {
                snapshot.Stubs.Add(localVariableDeclaration);
                return;
            }

            if (element.Type.ToString().StartsWith("Moq.Mock`1[T -> "))
            {
                snapshot.Mocks.Add(localVariableDeclaration);
                EatMoqMock(snapshot, localVariableDeclaration);
                return;
            }

            snapshot.TargetCandidates.Add(localVariableDeclaration);
            EatTargetCandidate(snapshot, localVariableDeclaration);
        }

        private void EatInvocationExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration, IInvocationExpression invocationExpression)
        {
            var invokedMethod = invocationExpression.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;
            if (invokedMethod.ToString().StartsWith("Method:Moq.Mock.Of()"))
            {
                snapshot.Stubs.Add(localVariableDeclaration);
                return;
            }

            // TODO : eat some variable instances that create by method invocation
        }

        private void EatReferenceExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {
            
        }

        private void EatEqualityExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {

        }

        private void EatUnaryExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {

        }

        private void EatBinaryExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {

        }

        private void EatConditionalTernaryExpression(Snapshot snapshot, ILocalVariableDeclaration localVariableDeclaration)
        {

        }
        
        #endregion

        #region Eat Target

        private void EatTargetCandidate(Snapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            var к = variableDeclaration;
        }
        
        #endregion

        #region Eat Moq Stub

        private void EatMoqStubMock(Snapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            var mock = variableDeclaration.DeclaredElement;
        }
        
        #endregion

        #region Eat Moq Mock

        private void EatMoqMock(Snapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            var mock = variableDeclaration.DeclaredElement;
        }
        
        #endregion
        
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

    }
}
