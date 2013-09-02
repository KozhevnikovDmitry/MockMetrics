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

            return method.HasAttributeInstance(new ClrTypeName("NUnit.Framework.TestAttribute"), false);
        }

        private void ProcessUnitTest(IMethodDeclaration unitTest)
        {

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