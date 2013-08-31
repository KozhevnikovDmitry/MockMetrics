using System.Collections.Generic;
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

            if (element == null)
                return;

            if (IsNunitTestDeclaration(methodDeclaration))
            {
                ProcessUnitTest(methodDeclaration);
            }
        }

        private bool IsNunitTestDeclaration(IMethodDeclaration method)
        {
            var isTest =
                method.Attributes.Any(t => t.Name.ShortName == "Test" || t.Name.ShortName == "TestCase");

            if (!isTest)
            {
                return false;
            }

            var classTests = method.GetContainingTypeDeclaration();
            var isFixtureClass = classTests.Attributes.Any(t => t.Name.ShortName == "TestFixture");
            if (!isFixtureClass)
            {
                return false;
            }

            var nameSpaceTests = classTests.GetContainingNamespaceDeclaration();
            
	    // todo: include nested classes
            var fileTests = classTests.GetContainingFile() as ICSharpTypeAndNamespaceHolderDeclaration;

            var hasSpaceNunitUsing =
                nameSpaceTests.Imports.Any(t => t.ImportedSymbolName.QualifiedName == "NUnit.Framework");

            var hasFileNunitUsing = fileTests != null &&
                                    fileTests.Imports.Any(
                                        t => t.ImportedSymbolName.QualifiedName == "NUnit.Framework");

            return hasSpaceNunitUsing || hasFileNunitUsing;
        }

        private void ProcessUnitTest(IMethodDeclaration unitTest)
        {
            int i = 0;
        }

        public bool ProcessingIsFinished
        {
            get
            {
                return _process.InterruptFlag;
            }

        }
    }
}