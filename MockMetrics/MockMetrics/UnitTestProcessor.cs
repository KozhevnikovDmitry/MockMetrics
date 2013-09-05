using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace MockMetrics
{
    public class UnitTestProcessor
    {
        public Snapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            var snapshot = new Snapshot();
            var subTree = unitTest.EnumerateSubTree();

            EatConstants(snapshot, subTree);
            EatLocalVariables(snapshot, subTree);
            EatExpressions(snapshot, subTree);

            return snapshot;
        }

        #region EatConstants
        
        private void EatConstants(Snapshot snapshot, IEnumerable<ITreeNode> testSubTree)
        {
            var variables = testSubTree.OfType<IMultipleConstantDeclaration>();

            foreach (var variable in variables)
            {
                foreach (var constantDeclaration in variable.EnumerateSubTree().OfType<ILocalConstantDeclaration>())
                {
                    snapshot.Constants.Add(constantDeclaration);
                }
            }
        }

        #endregion

        #region Eat Variables

        private void EatLocalVariables(Snapshot snapshot, IEnumerable<ITreeNode> testSubTree)
        {
            var variables = testSubTree.OfType<IMultipleLocalVariableDeclaration>();

            foreach (var variable in variables)
            {
                foreach (var localVariableDeclaration in variable.EnumerateSubTree().OfType<ILocalVariableDeclaration>())
                {
                    EatLocalVariable(snapshot, localVariableDeclaration);
                }
            }
        }

        private void EatLocalVariable(Snapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration.Initial is IArrayInitializer)
            {
                snapshot.Stubs.Add(variableDeclaration);
                return;
            }

            if (variableDeclaration.Initial is IExpressionInitializer)
            {
                var initializer = variableDeclaration.Initial as IExpressionInitializer;
                if (initializer.Value is ICSharpLiteralExpression)
                {
                    snapshot.Stubs.Add(variableDeclaration);
                    return;
                }

                if (initializer is IObjectCreationExpression)
                {
                    EatNewVariable(snapshot, variableDeclaration, initializer as IObjectCreationExpression);

                }

                if (initializer is IInvocationExpression)
                {
                    EatResultVariable(snapshot, variableDeclaration, initializer as IInvocationExpression);

                }
            }
        }

        private void EatNewVariable(Snapshot snapshot, 
                                    ILocalVariableDeclaration variableDeclaration,
                                    IObjectCreationExpression initializer)
        {
            var constructedType = initializer.TypeReference.CurrentResolveResult.DeclaredElement;

            if (constructedType.Type().Classify == TypeClassification.VALUE_TYPE)
            {
                snapshot.Stubs.Add(variableDeclaration);
            }

            if (constructedType.ToString() == "Class:Moq.Mock`1")
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
            
        }

        private void EatMock(Snapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {

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

        private void EatExpressions(Snapshot snapshot, IEnumerable<ITreeNode> testSubTree)
        {
            var expressions = testSubTree.OfType<IExpressionStatement>();
            foreach (var expression in expressions)
            {
                EatExpression(snapshot, expression);
            }
        }

        private void EatExpression(Snapshot snapshot, IExpressionStatement expression)
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
        
        #endregion
    }
}
