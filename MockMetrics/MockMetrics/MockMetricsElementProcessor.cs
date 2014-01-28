using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating;

namespace MockMetrics
{
    public class MockMetricsElementProcessor : IRecursiveElementProcessor
    {
        private readonly IDaemonProcess _process;

        private List<HighlightingInfo> _highlightings;
        private readonly UnitTestEater _eater;

        public List<HighlightingInfo> Highlightings
        {
            get
            {
                return _highlightings;
            }
        }

        public MockMetricsElementProcessor(IDaemonProcess process)
        {
            _eater = EatingRoot.Instance.GetUnitTestEater();
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
                var snapshot = _eater.EatUnitTest(methodDeclaration);
                Highlightings.Add(new HighlightingInfo(methodDeclaration.GetNameDocumentRange(), new MockMetricInfo(snapshot)));
            }
        }

        private bool IsNunitTestDeclaration(IMethod method)
        {
            if (method == null)
            {
                return false;
            }

            return method.HasAttributeInstance(new ClrTypeName("NUnit.Framework.TestAttribute"), false)
                || method.HasAttributeInstance(new ClrTypeName("NUnit.Framework.TestCaseAttribute"), false)
                || method.HasAttributeInstance(new ClrTypeName("Xunit.FactAttribute"), false);
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