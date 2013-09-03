using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace MockMetrics
{
    public class MockMetricsElementProcessor : IRecursiveElementProcessor
    {
        private readonly IDaemonProcess _process;

        private List<HighlightingInfo> _highlightings;

        public List<HighlightingInfo> Highlightings
        {
            get
            {
                return _highlightings;
            }
        }

        public MockMetricsElementProcessor(IDaemonProcess process, int arrangeAmount)
        {
            _process = process;
            _highlightings = new List<HighlightingInfo>();
        }

        public bool InteriorShouldBeProcessed(ITreeNode element)
        {
            return true;
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {

        }

        public void ProcessAfterInterior(ITreeNode element)
        {
            var methodDeclaration = element as IMethodDeclaration;

            if (methodDeclaration == null)
                return;

            if (methodDeclaration.IsAbstract)
                return;

            if (IsNunitTestDeclaration(methodDeclaration.DeclaredElement))
            {
                ProcessUnitTest(methodDeclaration);
            }
        }

        private bool IsNunitTestDeclaration(IMethod method)
        {
            if (method == null)
            {
                return false;
            }

            return method.HasAttributeInstance(new ClrTypeName("NUnit.Framework.TestAttribute"), false)
                || method.HasAttributeInstance(new ClrTypeName("NUnit.Framework.TestCaseAttribute"), false);
        }

        private void ProcessUnitTest(IMethodDeclaration unitTest)
        {
            var snapshot = new Snapshot();
            var subTree = unitTest.EnumerateSubTree();

            var variables = subTree.OfType<IMultipleLocalVariableDeclaration>();
            var constants = subTree.OfType<IMultipleLocalConstantDeclaration>();
            
            foreach (var variable in variables)
            {
                ProcessLocalVariable(snapshot, variable);
            }

            Highlightings.Add(new HighlightingInfo(unitTest.GetNameDocumentRange(), new MockMetricInfo(snapshot)));
        }

        private void ProcessLocalVariable(Snapshot snapshot, IMultipleLocalVariableDeclaration declaration)
        {
            foreach (var localVariableDeclaration in declaration.EnumerateSubTree().OfType<ILocalVariableDeclaration>())
            {
                var localVariableType = localVariableDeclaration.DeclaredElement.Type;

                if (localVariableType.Classify == TypeClassification.VALUE_TYPE)
                {
                    snapshot.Stubs.Add(localVariableDeclaration);
                    continue;
                }

                if (localVariableDeclaration.Initial is IArrayInitializer)
                {
                    snapshot.Stubs.Add(localVariableDeclaration);
                    continue;
                }

                if (localVariableDeclaration.Initial is IExpressionInitializer)
                {
                    var expressionInitializer = localVariableDeclaration.Initial as IExpressionInitializer;
                    var value = expressionInitializer.Value;

                    if (value is IInvocationExpression)
                    {
                        var invocation = value as IInvocationExpression;
                        var invokedMethod = invocation.InvocationExpressionReference.CurrentResolveResult.DeclaredElement;
                        if (invokedMethod.ToString() == "Method:Moq.Mock.Of()")
                        {
                            snapshot.Stubs.Add(localVariableDeclaration);
                            continue;
                        }
                    }

                    if (value is IObjectCreationExpression)
                    {
                        var objectCreation = value as IObjectCreationExpression;
                        var constructedType = objectCreation.TypeReference.CurrentResolveResult.DeclaredElement;
                        if (constructedType.ToString() == "Class:Moq.Mock`1")
                        {
                            snapshot.Mocks.Add(localVariableDeclaration);
                            continue;
                        }

                        snapshot.Targets.Add(localVariableDeclaration);
                    }
                }




            }
        }



        public bool ProcessingIsFinished
        {
            get
            {
                return _process.InterruptFlag;
            }

        }
    }

    public class Snapshot
    {
        public Snapshot()
        {
            Targets = new List<ILocalVariableDeclaration>();
            Stubs = new List<ILocalVariableDeclaration>();
            Mocks = new List<ILocalVariableDeclaration>();
        }

        public List<ILocalVariableDeclaration> Targets { get; set; }

        public List<ILocalVariableDeclaration> Stubs { get; set; }

        public List<ILocalVariableDeclaration> Mocks { get; set; }
    }
}