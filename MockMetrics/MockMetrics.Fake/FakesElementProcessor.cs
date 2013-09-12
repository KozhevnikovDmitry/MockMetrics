using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace MockMetrics.Fake
{
    public class FakesElementProcessor : IRecursiveElementProcessor
    {
        public static List<IMethodDeclaration> UnitTestDeclarations = new List<IMethodDeclaration>();

        private readonly IDaemonProcess _process;

        private List<HighlightingInfo> _highlightings;

        public List<HighlightingInfo> Highlightings
        {
            get
            {
                return _highlightings;
            }
        }

        public FakesElementProcessor(IDaemonProcess process)
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
                UnitTestDeclarations.Add(methodDeclaration);
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

        public bool ProcessingIsFinished
        {
            get
            {
                return _process.InterruptFlag;
            }

        }
    }
}