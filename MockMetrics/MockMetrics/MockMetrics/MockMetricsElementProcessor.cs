using System;
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
            if (element is IMethodDeclaration)
            {
                var methodDeclaration = element as IMethodDeclaration;
                var isNunitTest =
                    methodDeclaration.Attributes.Any(t => t.Name.ShortName == "Test" || t.Name.ShortName == "TestCase");
                if (isNunitTest)
                {
                    var classTests = methodDeclaration.GetContainingTypeDeclaration();
                    var nameSpaceTests = classTests.GetContainingNamespaceDeclaration();
                    var fileTests = classTests.GetContainingFile() as ICSharpTypeAndNamespaceHolderDeclaration;

                    var hasSpaceNunitUsing = nameSpaceTests.Imports.Any(t => t.ImportedSymbolName.QualifiedName == "NUnit.Framework");
                    var hasFileNunitUsing = fileTests != null && fileTests.Imports.Any(t => t.ImportedSymbolName.QualifiedName == "NUnit.Framework");
                    var isFixtureClass = classTests.Attributes.Any(t => t.Name.ShortName == "TestFixture");

                    if (isFixtureClass && (hasSpaceNunitUsing || hasFileNunitUsing))
                    {
                        ProcessUnitTest(methodDeclaration);
                    }
                }
            }
        }

        private void ProcessUnitTest(ICSharpFunctionDeclaration funcDeclaration)
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